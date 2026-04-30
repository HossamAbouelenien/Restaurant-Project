using System.Text.Json.Serialization;

namespace RMS.Shared.DTOs.AiDTOs
{
    public class SuggestResponseDTO
    {

        [JsonPropertyName("results")]
        public List<SuggestResultDTO> Results { get; set; } = new();

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }

}
