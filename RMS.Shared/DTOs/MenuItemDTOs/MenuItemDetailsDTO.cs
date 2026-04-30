using RMS.Shared.DTOs.RecipeDTOs;

namespace RMS.Shared.DTOs.MenuItemDTOs
{
    public class MenuItemDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int PrepTimeMinutes { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
        public string CategoryArabicName { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public List<RecipesListDTO> Recipes { get; set; } = new();
    }
}
