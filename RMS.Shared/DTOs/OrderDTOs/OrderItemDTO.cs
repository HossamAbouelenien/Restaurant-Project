namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
        public int? OrderItemId { get; set; }
        public int MenuItemId { get; set; }
        public string? MenuItemName { get; set; }
        public string? ArabicMenuItemName { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Notes { get; set; }
        
    }
}
