using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.DeliveryDTOs
{
    public class OrderItemDto
    {
        public string MenuItemName { get; set; } = default!;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Total => Quantity * UnitPrice;

        public string? Notes { get; set; }
    }
}
