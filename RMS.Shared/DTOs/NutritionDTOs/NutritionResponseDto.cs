using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.NutritionDTOs
{
    public class NutritionResponseDto
    {
        public List<NutritionItemResultDto> Items { get; set; } = new();
        public NutritionTotalsDto OrderTotals { get; set; } = new();
    }
}
