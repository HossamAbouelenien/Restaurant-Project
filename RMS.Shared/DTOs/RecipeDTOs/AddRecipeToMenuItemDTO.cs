using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.RecipeDTOs
{
    public class AddRecipeToMenuItemDTO
    {
        public int MenuItemId { get; set; }
        public int IngredientId { get; set; }
        public decimal QuantityRequired { get; set; }
    }
}
