using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.DTOs.MenuItemDTOs
{
    public class CreateMenuItemDTO
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string ArabicName { get; set; } = string.Empty;

        [Required]
        [Range(1, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 300)]
        public int PrepTimeMinutes { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public IFormFile? Image { get; set; } = default!;
        public string? ImagePublicId { get; set; }

        [Required]
        public List<CreateRecipeDTO> Recipes { get; set; } = [];


    }
}
