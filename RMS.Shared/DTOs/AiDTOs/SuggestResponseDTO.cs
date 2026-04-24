using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.AiDTOs
{
    public class SuggestResponseDTO
    {
        public List<SuggestResultDTO> Results { get; set; } = new();
        public int TotalResults { get; set; }
    }

}
