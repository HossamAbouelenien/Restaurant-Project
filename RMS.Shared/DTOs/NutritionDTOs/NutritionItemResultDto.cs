using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.NutritionDTOs
{
    public class NutritionItemResultDto
    {
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public NutritionMacrosDto PerItem { get; set; } = new();
        public NutritionMacrosDto Totals { get; set; } = new();
    }
}
