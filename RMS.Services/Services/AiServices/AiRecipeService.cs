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
            var response = await _httpClient.PostAsJsonAsync("/suggest/internal", request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<SuggestResponseDTO>(json, options)
                   ?? new SuggestResponseDTO();
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
