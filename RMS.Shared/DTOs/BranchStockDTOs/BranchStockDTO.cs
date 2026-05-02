namespace RMS.Shared.DTOs.BranchStockDTOs
{
    public class BranchStockDTO
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? BranchArabicName { get; set; }


        public string? IngredientArabicName { get; set; }

        public string? Unit { get; set; }
        public int IngredientId { get; set; }
        public string? IngredientName { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal LowThreshold { get; set; }

    }
}
