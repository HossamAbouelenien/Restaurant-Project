namespace RMS.Shared.DTOs.OrderDTOs
{
    public class MyDeliveryActiveCustomersDTO
    {
        public int Id { get; set; } = default!;
        public int BranchId { get; set; }
        public string BranchName { get; set; } = default!;
        public string OrderType { get; set; } = default!;
        public string Status { get; set; } = default!;
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
        public OrderDeliveryDTO? Delivery { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
