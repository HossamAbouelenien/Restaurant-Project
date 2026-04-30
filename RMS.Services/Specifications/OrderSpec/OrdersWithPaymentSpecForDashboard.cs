using RMS.Domain.Entities;

namespace RMS.Services.Specifications.OrderSpec
{
    public class OrdersWithPaymentSpecForDashboard : BaseSpecifications<Order>
    {
        public OrdersWithPaymentSpecForDashboard()
            : base(o => !o.IsDeleted)
        {
            AddInclude(o => o.Payment!);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.Branch!);
        }
    }
}
