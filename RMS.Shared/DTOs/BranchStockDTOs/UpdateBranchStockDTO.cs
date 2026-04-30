using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.DTOs.BranchStockDTOs
{
    public class UpdateBranchStockDTO
    {
        [Range(0, 999999)]
        public decimal QuantityAvailable { get; set; }

        [Range(0, 999999)]
        public decimal LowThreshold { get; set; }
    }
}
