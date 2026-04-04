using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.RecipeDTOs
{
    public class RecipeDTO
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal QuantityRequired { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
