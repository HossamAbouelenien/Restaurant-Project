using RMS.Domain.Entities;
using RMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderDeliveryDTO
    {
        public string DriverId { get; set; } = default!;
        public string? DriverName { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string DeliveryStatus { get; set; }=default!;
        public decimal? CashCollected { get; set; }
        public OrderAddressDTO DeliveryAddress { get; set; } =default!;
    }
}
