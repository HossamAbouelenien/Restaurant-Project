using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.DeliverySpec
{
    public class DeliveriesWithOrderSpecification : BaseSpecifications<Delivery>
    {
        public DeliveriesWithOrderSpecification(DeliveryQueryParams queryParams)
            : base(b =>
                (!queryParams.BranchId.HasValue || b.Order!.BranchId == queryParams.BranchId.Value)
                && (!queryParams.OrderId.HasValue || b.OrderId == queryParams.OrderId.Value)
                && (!queryParams.Status.HasValue || b.DeliveryStatus == queryParams.Status.Value)
                && (!queryParams.Date.HasValue || b.CreatedAt.Date == queryParams.Date.Value.Date)
            )
        {
            AddInclude(d => d.Order!);
            AddInclude(d => d.Order!.Branch!);
            AddInclude(d => d.Order!.User!);
            AddInclude(d => d.Order!.OrderItems!);
            AddInclude(d => d.Driver!);
            AddInclude(d => d.DeliveryAddress);
            AddInclude("Order.OrderItems.MenuItem");

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public DeliveriesWithOrderSpecification(string driverId)
       : base(d => d.DriverId == driverId)
        {
            AddInclude(d => d.Order!);
            AddInclude(d => d.Order!.Branch!);
            AddInclude(d => d.Order!.OrderItems!);
            AddInclude(d => d.Driver!);
            AddInclude(d => d.DeliveryAddress);
            AddInclude("Order.OrderItems.MenuItem");
        }
    }
}
