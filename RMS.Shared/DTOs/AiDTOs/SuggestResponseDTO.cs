using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
