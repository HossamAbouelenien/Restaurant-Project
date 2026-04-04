using RMS.Shared.DTOs.RecipeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.MenuItemDTOs
{
    public class MenuItemDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int PrepTimeMinutes { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public List<RecipeDTO> Recipes { get; set; } = new();
    }
}
