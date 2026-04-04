using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.BranchStockDTOs
{
    public class BranchStockDTO
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int IngredientId { get; set; }
        public string? IngredientName { get; set; }
        public decimal QuantityAvailable { get; set; }
        public decimal LowThreshold { get; set; }

    }
}
