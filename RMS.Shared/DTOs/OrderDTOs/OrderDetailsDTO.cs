using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; } = default!;
        public string BranchName { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string UserRole { get; set; } = default!;
        //public string UserRoleName { get; set; } = default!;
        public string OrderType { get; set; } = default!;
        public string Status { get; set; } = default!;
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public List<OrderKitchenTicketsDTO> KitchenTickets { get; set; } = new List<OrderKitchenTicketsDTO>();
        public OrderPaymentDTO? Payment { get; set; }      
        public OrderDeliveryDTO? Delivery { get; set; }     

        public decimal TotalAmount { get; set; }
        public string? Tablenumber { get; set; }
    }
}
