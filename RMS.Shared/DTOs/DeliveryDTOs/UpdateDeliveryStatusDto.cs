using RMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.DeliveryDTOs
{
    public class UpdateDeliveryStatusDto
    {
        [Required]
        public string Status { get; set; } = default!;

        public decimal? CashCollected { get; set; }
    }
}
