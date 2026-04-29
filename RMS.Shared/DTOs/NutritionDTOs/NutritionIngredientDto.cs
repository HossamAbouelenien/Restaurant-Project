using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.NutritionDTOs
{
    public class NutritionIngredientDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal QuantityRequired { get; set; }
        public string Unit { get; set; } = string.Empty; // gram, kg, piece, liter
    }
}
