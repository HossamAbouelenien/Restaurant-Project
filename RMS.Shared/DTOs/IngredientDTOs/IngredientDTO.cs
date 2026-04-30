namespace RMS.Shared.DTOs.IngredientDTOs
{
    public class IngredientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public string Unit { get; set; }
    }
}
