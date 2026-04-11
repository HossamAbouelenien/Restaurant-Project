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

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public List<RecipesListDTO> Recipes { get; set; } = new();
    }
}
