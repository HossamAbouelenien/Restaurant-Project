using RMS.Domain.Entities;

namespace RMS.Services.Specifications.KitchenTicketSpec
{
    public class KitchenTicketWithOrderSpecification : BaseSpecifications<KitchenTicket>
    {
        public KitchenTicketWithOrderSpecification(int ticketId)
            : base(t => t.Id == ticketId)
        {
            AddInclude(o=>o.Order!);
        }
        
    }
}
