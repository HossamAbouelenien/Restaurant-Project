using Microsoft.Extensions.Logging;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Entities.CustomerBasket;
using RMS.Services.AiServices.Helper;
using RMS.Services.Exceptions.Base;
using RMS.ServicesAbstraction.IAiServices;
using RMS.Shared.DTOs.NutritionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RMS.Services.AiServices.NutritionServices
{
    public class NutritionService : INutritionService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly INutritionRepository _nutritionRepository;
        private readonly INutritionAiService _nutritionAiService;
        private readonly ILogger<NutritionService> _logger;

        public NutritionService(
            IBasketRepository basketRepository,
            INutritionRepository nutritionRepository,
            INutritionAiService nutritionAiService,
            ILogger<NutritionService> logger)
        {
            _basketRepository = basketRepository;
            _nutritionRepository = nutritionRepository;
            _nutritionAiService = nutritionAiService;
            _logger = logger;
        }

        public async Task<NutritionResponseDto> CalculateBasketNutritionAsync(
            string basketId,
            CancellationToken cancellationToken = default)
        {
            // Step 1: Get basket from Redis
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket is null)
                throw new Exception($"Basket '{basketId}' was not found.");

            if (basket.Items is null || basket.Items.Count == 0)
                throw new Exception("Basket is empty. Add items before calculating nutrition.");

            // Step 2: Extract MenuItemIds
            var menuItemIds = basket.Items.Select(i => i.Id).Distinct().ToList();

            // Step 3: Fetch MenuItems with Recipes + Ingredients
            var menuItems = await _nutritionRepository.GetMenuItemsWithIngredientsAsync(menuItemIds, cancellationToken);

            if (menuItems is null || menuItems.Count == 0)
                throw new Exception("No menu items found for the given basket.");

            // Step 4: Build structured DTO list for prompt builder
            var nutritionItems = BuildNutritionItems(basket.Items, menuItems);

            // Step 5: Build prompt
            var prompt = NutritionPromptBuilder.Build(nutritionItems);

            _logger.LogInformation("Sending nutrition prompt to AI for basket {BasketId}", basketId);

            // Step 6: Call AI (one single request)
            string rawJson;
            try
            {
                rawJson = await _nutritionAiService.GetNutritionJsonAsync(prompt, cancellationToken);
            }
            // ✅ مؤقتاً عشان نشوف الـ error الحقيقي
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI service failed for basket {BasketId}", basketId);
                throw new Exception($"AI FAILED: {ex.Message} | Inner: {ex.InnerException?.Message}");
            }

            // Step 7: Parse and return structured response
            return ParseAiResponse(rawJson);
        }

        private static List<NutritionBasketItemDto> BuildNutritionItems(
            ICollection<BasketItem> basketItems,
            List<MenuItem> menuItems)
        {
            var menuItemMap = menuItems.ToDictionary(m => m.Id);
            var result = new List<NutritionBasketItemDto>();

            foreach (var basketItem in basketItems)
            {
                if (!menuItemMap.TryGetValue(basketItem.Id, out var menuItem))
                {
                    // Skip items not found — log-worthy but non-fatal
                    continue;
                }

                var ingredients = menuItem.Recipes?
                    .Where(r => r.Ingredient is not null)
                    .Select(r => new NutritionIngredientDto
                    {
                        Name = r.Ingredient!.Name,
                        QuantityRequired = r.QuantityRequired,
                        Unit = r.Ingredient.Unit.ToString(),
                    })
                    .ToList() ?? new List<NutritionIngredientDto>();

                result.Add(new NutritionBasketItemDto
                {
                    MenuItemId = menuItem.Id.ToString(),
                    Quantity = basketItem.Quantity,
                    ItemName = menuItem.Name,
                    Ingredients = ingredients
                });
            }

            return result;
        }

        private static NutritionResponseDto ParseAiResponse(string rawJson)
        {
            try
            {
                var cleaned = rawJson.Trim();

               
                if (cleaned.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
                    cleaned = cleaned["```json".Length..];
                else if (cleaned.StartsWith("```"))
                    cleaned = cleaned[3..];

                if (cleaned.EndsWith("```"))
                    cleaned = cleaned[..^3];

                cleaned = cleaned.Trim();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<NutritionResponseDto>(cleaned, options);
                return result ?? throw new InvalidOperationException("AI returned null nutrition data.");
            }
            catch (JsonException ex)
            {
                throw new Exception($"AI returned invalid JSON: {ex.Message}");
            }
        }
    }
}
