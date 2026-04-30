namespace RMS.Shared.DTOs.KitchenDTOs
{
    public class KitchenBoardDto
    {
        public List<OrderKitchenTicketDTO> Pending { get; set; } = new();
        public List<OrderKitchenTicketDTO> Preparing { get; set; } = new();
        public List<OrderKitchenTicketDTO> Done { get; set; } = new();
    }
}
