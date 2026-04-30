using RMS.Domain.Entities;

namespace RMS.Services.Specifications.DeliverySpec
{
    public class DeliveryByOrderIdSpecification : BaseSpecifications<Delivery>
    {
        public DeliveryByOrderIdSpecification(int orderId)
            : base(d => d.OrderId == orderId)
        {
        }
    }
}
