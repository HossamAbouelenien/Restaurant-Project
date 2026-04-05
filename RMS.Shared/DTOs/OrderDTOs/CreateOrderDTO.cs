using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {
        public string? CustomerId { get; set; }
        public int BranchId { get; set; }
        public string OrderType { get; set; } = default!;
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }
}
