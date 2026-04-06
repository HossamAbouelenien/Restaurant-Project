using RMS.Domain.Enums;
using RMS.Shared.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.DeliveryDTOs
{
    public class DeliveryDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string? DriverId { get; set; }
        public string? DriverName { get; set; }

        public int BranchId { get; set; }
        public string? BranchName { get; set; }

        public DeliveryStatus DeliveryStatus { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public decimal? CashCollected { get; set; }

        public AddressDto DeliveryAddress { get; set; } = default!;
    }
}
