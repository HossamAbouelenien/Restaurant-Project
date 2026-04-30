using System.Text.Json.Serialization;

namespace RMS.Services.Services.AiServices.NutritionServices.Models
{
    internal class OpenAiChatRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("messages")]
        public List<OpenAiMessage> Messages { get; set; } = new();

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("response_format")]
        public ResponseFormat? ResponseFormat { get; set; }
    }
}
