using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
