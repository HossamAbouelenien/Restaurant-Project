using RMS.Domain.Entities;

namespace RMS.Services.Specifications.KitchenTicketSpec
{
    public class TicketByOrderSpecification : BaseSpecifications<KitchenTicket>
    {
        public TicketByOrderSpecification(int orderId)
            : base(t => t.OrderId == orderId)
        {
        }
    }
}
