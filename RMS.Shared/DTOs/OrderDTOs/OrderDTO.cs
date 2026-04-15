using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderDTO
    {
        public int Id { get; set; } = default!;
        public string BranchName { get; set; }=default!;
        public string CustomerId { get; set; } = default!;
        public string CustomerName { get; set; } = default!;
        public string OrderType { get; set; } = default!;
        public string Status { get; set; } = default!;
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public decimal TotalAmount { get; set; }
        public string? TableNumber { get; set; }

        public string? PaymentMethod { get; set; }
        public string? PaymentStatus { get; set; }


    }
}
