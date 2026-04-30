using RMS.Domain.Entities;

namespace RMS.Services.Specifications.DeliverySpec
{
    public class OrderByIdSpecification : BaseSpecifications<Order>
    {
        public OrderByIdSpecification(int orderId)
            : base(o => o.Id == orderId)
        {
            AddInclude(o => o.Delivery!);
        }
    }
}
