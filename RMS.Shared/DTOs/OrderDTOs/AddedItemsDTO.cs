namespace RMS.Shared.DTOs.OrderDTOs
{
    public class AddedItemsDTO
    {
        public int OrderID { get; set; }
        public List<CreateOrderItemDTO> AddedItems { get; set; } = new List<CreateOrderItemDTO>();
    }
}
