using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction.IAiServices;
using RMS.Shared.DTOs.AiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.AiServices
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
            return await response.Content.ReadFromJsonAsync<SuggestResponseDTO>()
                   ?? new SuggestResponseDTO();
        }

        public async Task SyncRecipesToAiAsync()
        {
            var repo = _unitOfWork.GetRepository<Recipe>();
            var recipes = await repo.GetAllAsync();

            var grouped = recipes
                .GroupBy(r => new { r.MenuItemId, r.MenuItem!.Name })
                .Select(g => new
                {
                    menu_item_id = g.Key.MenuItemId,
                    menu_item_name = g.Key.Name,
                    ingredients = g.Select(r => r.Ingredient!.Name).ToList()
                });

            var payload = new { recipes = grouped };

            var response = await _httpClient.PostAsJsonAsync("/admin/load-recipes", payload);
            response.EnsureSuccessStatusCode();
        }
    }
}
