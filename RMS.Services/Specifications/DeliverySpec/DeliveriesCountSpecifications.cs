using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.DeliverySpec;

public class DeliveriesCountSpecifications : BaseSpecifications<Delivery>
{
    public DeliveriesCountSpecifications(DeliveryQueryParams queryParams)
        : base(b =>
            (!queryParams.BranchId.HasValue || b.Order!.BranchId == queryParams.BranchId.Value)
            && (!queryParams.OrderId.HasValue || b.OrderId == queryParams.OrderId.Value)
            && (!queryParams.Status.HasValue || b.DeliveryStatus == queryParams.Status.Value)
            && (!queryParams.Date.HasValue || b.CreatedAt.Date == queryParams.Date.Value.Date)
        )
    {
    }
}
