using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        public string? UserId { get; set; }
        public int BranchId { get; set; }
        public string OrderType { get; set; } = default!;
        public List<CreateOrderItemDTO> Items { get; set; } = new List<CreateOrderItemDTO>();
        public int? TableId { get; set; }
        public OrderAddressDTO? DeliveryAddress { get; set; }
        public string PaymentMethod { get; set; } = default!;
    }
}
