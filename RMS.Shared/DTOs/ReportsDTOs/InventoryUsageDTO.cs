using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.ReportsDTOs
{
    public class InventoryUsageDTO
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string IngredientArabicName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal QuantityUsed { get; set; }
        public string Unit { get; set; } = string.Empty;

    }
}
