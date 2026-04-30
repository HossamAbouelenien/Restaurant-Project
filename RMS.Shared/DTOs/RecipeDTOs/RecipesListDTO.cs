namespace RMS.Shared.DTOs.RecipeDTOs
{
    public class RecipesListDTO
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = default!;
        public string MenuItemArabicName { get; set; } = default!;
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = default!;
        public string IngredientArabicName { get; set; } = default!;
        public decimal QuantityRequired { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}
