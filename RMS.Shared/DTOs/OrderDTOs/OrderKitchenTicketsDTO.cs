namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderKitchenTicketsDTO
    {
            public int Id { get; set; }

            public string Station { get; set; } = string.Empty;

            public string Status { get; set; }=default!;

            public DateTime? StartedAt { get; set; }

            public DateTime? CompletedAt { get; set; }
        
    }
}
