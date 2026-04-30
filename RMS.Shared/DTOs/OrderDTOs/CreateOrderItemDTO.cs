namespace RMS.Shared.DTOs.OrderDTOs
{
    public class CreateOrderItemDTO
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Notes { get; set; }
    }
}
