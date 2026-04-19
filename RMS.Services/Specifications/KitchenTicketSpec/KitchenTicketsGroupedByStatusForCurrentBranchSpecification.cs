using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.KitchenTicketSpec
{
    public class KitchenTicketsGroupedByStatusForCurrentBranchSpecification : BaseSpecifications<KitchenTicket>
    {
        public KitchenTicketsGroupedByStatusForCurrentBranchSpecification(int id) : base(b => b.Id == id && (!b.ConfirmedServed))
        {
            AddInclude(t => t.Order!);
            AddInclude("Order.OrderItems");
            AddInclude("Order.OrderItems.MenuItem");
            
        }
        public KitchenTicketsGroupedByStatusForCurrentBranchSpecification(KitchenTicketQueryParams queryParams)
            : base(b =>
            (!queryParams.branchId.HasValue || b.Order!.BranchId == queryParams.branchId.Value)
            && (!queryParams.OrderId.HasValue || b.OrderId == queryParams.OrderId.Value)
            && (string.IsNullOrEmpty(queryParams.Station) || b.Station == queryParams.Station)
            && (!queryParams.Status.HasValue || b.Status == queryParams.Status.Value)
            && (!b.ConfirmedServed))
        
        {
            
            AddInclude(b => b.Order!);
        }
    }
}
