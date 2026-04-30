using RMS.Domain.Entities;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class OrderWithItemsAndPaymentAndDeliveryAndKitchenTicketsSpecification : BaseSpecifications<Order>
    {
        public OrderWithItemsAndPaymentAndDeliveryAndKitchenTicketsSpecification(int id) : base(o => o.Id == id)
        {
            AddInclude(o => o.Branch!);
            AddInclude(o => o.User!);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.Payment!);
            AddInclude(o => o.KitchenTickets);
            AddInclude("Delivery.Driver");
            AddInclude("TableOrder.Table");
            AddInclude("OrderItems.MenuItem");
            //AddInclude("Customer.Role");

        }
    }
}
