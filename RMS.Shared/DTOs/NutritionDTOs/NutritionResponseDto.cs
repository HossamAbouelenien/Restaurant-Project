namespace RMS.Shared.DTOs.NutritionDTOs
{
    public class NutritionResponseDto
    {
        public List<NutritionItemResultDto> Items { get; set; } = new();
        public NutritionTotalsDto OrderTotals { get; set; } = new();
    }
}
