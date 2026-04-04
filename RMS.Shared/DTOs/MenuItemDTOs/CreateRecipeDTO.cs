using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.MenuItemDTOs
{
    public class CreateRecipeDTO
    {
        [Required]
        public int IngredientId { get; set; }

        [Required]
        //[Range(0.001, 9999)]
        public decimal QuantityRequired { get; set; }
    }
}
