using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.MenuItemDTOs
{
    public class UpdateMenuItemDTO
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string ArabicName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 300)]
        public int PrepTimeMinutes { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        // null = keep existing image | new file = replace image
        public IFormFile? Image { get; set; }

        [Required]
        public List<CreateRecipeDTO> Recipes { get; set; } = [];
    }
}
