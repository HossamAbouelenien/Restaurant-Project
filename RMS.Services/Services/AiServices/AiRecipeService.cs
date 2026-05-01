using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction.IServices.IAiServices;
using RMS.Shared.DTOs.AiDTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace RMS.Services.Services.AiServices
{
    public class AiRecipeService : IAiRecipeService
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public AiRecipeService(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClientFactory.CreateClient("RecipeSuggestionClient");
            _unitOfWork = unitOfWork;
        }

        public async Task<SuggestResponseDTO> SuggestRecipesAsync(SuggestRequestDTO request)
        {
            var userIngredients = request.Ingredients?
                .Select(i => i.ToLower())
                .ToList() ?? new List<string>();

            var response = await _httpClient.PostAsJsonAsync("/suggest/internal", request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<SuggestResponseDTO>(json, options)
                         ?? new SuggestResponseDTO();

            
            if (result.Results != null && result.Results.Any())
            {
                result.Results = result.Results
                    .Where(r => r.MatchedIngredients != null &&
                                r.MatchedIngredients.Any(i =>
                                    userIngredients.Contains(i.ToLower())))
                    .OrderByDescending(r => r.MatchScore)
                    .ToList();

                result.TotalResults = result.Results.Count;
            }

           
            if (result.Results == null || !result.Results.Any())
            {
                var repo = _unitOfWork.GetRepository<Recipe>();
                var menuItemRepo = _unitOfWork.GetRepository<MenuItem>();
                var ingredientRepo = _unitOfWork.GetRepository<Ingredient>();

                var recipes = await repo.GetAllAsync();
                var menuItems = await menuItemRepo.GetAllAsync();
                var ingredients = await ingredientRepo.GetAllAsync();

                var ingredientDict = ingredients.ToDictionary(i => i.Id, i => i.Name.ToLower());
                var menuItemDict = menuItems.ToDictionary(m => m.Id, m => m.Name);

                var matchedMenuItems = recipes
                    .Where(r => ingredientDict.ContainsKey(r.IngredientId) &&
                                userIngredients.Contains(ingredientDict[r.IngredientId]))
                    .Select(r => r.MenuItemId)
                    .Distinct()
                    .ToList();

                var fallbackResults = matchedMenuItems
                    .Select(id => new SuggestResultDTO
                    {
                        MenuItemId = id,
                        MenuItemName = menuItemDict.ContainsKey(id) ? menuItemDict[id] : "Unknown",
                        MatchScore = 0.3, 
                        MatchedIngredients = request.Ingredients,
                        MissingIngredients = new List<string>()
                    })
                    .ToList();

                result.Results = fallbackResults;
                result.TotalResults = fallbackResults.Count;
            }

            return result;
        }

        public async Task SyncRecipesToAiAsync()
        {
            var repo = _unitOfWork.GetRepository<Recipe>();
            var recipes = await repo.GetAllAsync();

            
            var menuItemRepo = _unitOfWork.GetRepository<MenuItem>();
            var ingredientRepo = _unitOfWork.GetRepository<Ingredient>();

            var menuItems = await menuItemRepo.GetAllAsync();
            var ingredients = await ingredientRepo.GetAllAsync();

            var menuItemDict = menuItems.ToDictionary(m => m.Id);
            var ingredientDict = ingredients.ToDictionary(i => i.Id);

            var grouped = recipes
                .Where(r => menuItemDict.ContainsKey(r.MenuItemId) && ingredientDict.ContainsKey(r.IngredientId))
                .GroupBy(r => new { r.MenuItemId, Name = menuItemDict[r.MenuItemId].Name })
                .Select(g => new
                {
                    menu_item_id = g.Key.MenuItemId,
                    menu_item_name = g.Key.Name,
                    ingredients = g.Select(r => ingredientDict[r.IngredientId].Name).ToList()
                }).ToList();

            var payload = new { recipes = grouped };

            var response = await _httpClient.PostAsJsonAsync("/admin/load-recipes", payload);
            response.EnsureSuccessStatusCode();
        }
    }
}
