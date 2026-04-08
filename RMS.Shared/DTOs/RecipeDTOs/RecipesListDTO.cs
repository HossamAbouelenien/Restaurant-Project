using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.RecipeDTOs
{
    public class RecipesListDTO
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = default!;
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = default!;
        public decimal QuantityRequired { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
