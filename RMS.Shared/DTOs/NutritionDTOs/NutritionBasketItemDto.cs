namespace RMS.Shared.DTOs.NutritionDTOs
{
    public class NutritionBasketItemDto
    {
        public string MenuItemId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public List<NutritionIngredientDto> Ingredients { get; set; } = new();
    }
}
