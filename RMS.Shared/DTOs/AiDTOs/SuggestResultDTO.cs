using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.AiDTOs
{
    public class SuggestResultDTO
    {
        [JsonPropertyName("menu_item_id")]
        public int MenuItemId { get; set; }

        [JsonPropertyName("menu_item_name")]
        public string MenuItemName { get; set; } = string.Empty;

        [JsonPropertyName("match_score")]
        public double MatchScore { get; set; }

        [JsonPropertyName("matched_ingredients")]
        public List<string> MatchedIngredients { get; set; } = new();

        [JsonPropertyName("missing_ingredients")]
        public List<string> MissingIngredients { get; set; } = new();
    }
}
