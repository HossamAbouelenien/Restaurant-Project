using RMS.Shared.DTOs.NutritionDTOs;
using System.Text;

namespace RMS.Services.Services.AiServices.Helper
{
    public class NutritionPromptBuilder
    {
        /// <summary>
        /// Converts basket items + their ingredients into a structured AI prompt.
        /// </summary>
        public static string Build(List<NutritionBasketItemDto> items)
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are a professional nutritionist and food scientist.");
            sb.AppendLine("Analyze the following meal order and calculate the nutritional values.");
            sb.AppendLine("For each menu item, you are given the quantity ordered and its ingredients with amounts.");
            sb.AppendLine();
            sb.AppendLine("INSTRUCTIONS:");
            sb.AppendLine("- Calculate calories, protein (g), and carbs (g) per single item.");
            sb.AppendLine("- Multiply per-item values by quantity to get totals for that item.");
            sb.AppendLine("- Sum all item totals to produce orderTotals.");
            sb.AppendLine("- Base calculations on standard nutritional databases (USDA or equivalent).");
            sb.AppendLine("- Return ONLY valid JSON. No explanations, no markdown, no extra text.");
            sb.AppendLine();
            sb.AppendLine("ORDER DETAILS:");
            sb.AppendLine();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                sb.AppendLine($"Item {i + 1}:");
                sb.AppendLine($"  Name: {item.ItemName}");
                sb.AppendLine($"  Quantity Ordered: {item.Quantity}");
                sb.AppendLine($"  Ingredients (per single serving):");

                foreach (var ingredient in item.Ingredients)
                {
                    var formatted = FormatIngredient(ingredient);
                    sb.AppendLine($"    - {formatted}");
                }

                sb.AppendLine();
            }

            sb.AppendLine("REQUIRED JSON FORMAT:");
            sb.AppendLine("""
        {
          "items": [
            {
              "itemName": "string",
              "quantity": number,
              "perItem": {
                "calories": number,
                "protein": number,
                "carbs": number
              },
              "totals": {
                "calories": number,
                "protein": number,
                "carbs": number
              }
            }
          ],
          "orderTotals": {
            "calories": number,
            "protein": number,
            "carbs": number
          }
        }
        """);

            return sb.ToString();
        }

        private static string FormatIngredient(NutritionIngredientDto ingredient)
        {
            return ingredient.Unit.ToLower() switch
            {
                "gram" or "grams" or "g" => $"{ingredient.QuantityRequired}g {ingredient.Name}",
                "kg" or "kilogram" or "kilograms" => $"{ingredient.QuantityRequired}kg {ingredient.Name}",
                "liter" or "liters" or "l" => $"{ingredient.QuantityRequired}L {ingredient.Name}",
                "piece" or "pieces" or "pcs" => $"{ingredient.QuantityRequired} piece(s) {ingredient.Name}",
                _ => $"{ingredient.QuantityRequired} {ingredient.Unit} {ingredient.Name}"
            };
        }
    }
}
