using RMS.Domain.Entities;

namespace RMS.Services.Specifications.PaymentSpec
{
    public class PaymentByOrderIdSpecification : BaseSpecifications<Payment>
    {
        public PaymentByOrderIdSpecification(int orderId)
            : base(p => p.OrderId == orderId)
        {
        }
    }
}
