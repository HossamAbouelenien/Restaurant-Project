namespace RMS.Shared.DTOs.MenuItemsDTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int PrepTimeMinutes { get; set; }
        public string Category { get; set; } = default!;
        public string CategoryArabicName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}
