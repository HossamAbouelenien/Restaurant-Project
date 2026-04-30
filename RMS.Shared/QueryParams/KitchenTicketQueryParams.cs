using RMS.Domain.Enums;

namespace RMS.Shared.QueryParams
{
    public class KitchenTicketQueryParams
    {
        public int? branchId { get; set; }
        public int? OrderId { get; set; }
        public string? Station { get; set; } = string.Empty;
        public TicketStatus? Status { get; set; } 
    }
}
