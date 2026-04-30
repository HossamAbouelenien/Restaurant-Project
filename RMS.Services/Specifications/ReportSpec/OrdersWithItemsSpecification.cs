using RMS.Domain.Entities;
using RMS.Domain.Enums;

namespace RMS.Services.Specifications.ReportSpec
{
    public class OrdersWithItemsSpecification : BaseSpecifications<Order>
    {
        public OrdersWithItemsSpecification()
            : base(o => !o.IsDeleted && o.Status != OrderStatus.Cancelled)
        {
            AddInclude(o => o.OrderItems);
            AddInclude("OrderItems.MenuItem");
        }
    }
}
