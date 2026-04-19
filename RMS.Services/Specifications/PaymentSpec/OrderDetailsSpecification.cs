using RMS.Domain.Entities;
using RMS.Services.Specifications;
using RMS.Shared.Enums;



public class OrderDetailsSpecification : BaseSpecifications<Order>
{
    public OrderDetailsSpecification(int orderId, OrderIncludes includes = OrderIncludes.None)
        : base(o => o.Id == orderId)
    {
        // ── Includes ─────────────────────────

        if (includes.HasFlag(OrderIncludes.User))
            AddInclude(o => o.User);

        if (includes.HasFlag(OrderIncludes.Payment))
            AddInclude(o => o.Payment);

        if (includes.HasFlag(OrderIncludes.OrderItems))
            AddInclude(o => o.OrderItems);

        if (includes.HasFlag(OrderIncludes.Delivery))
            AddInclude(o => o.Delivery);

        if (includes.HasFlag(OrderIncludes.TableOrder))
            AddInclude(o => o.TableOrder);

        if (includes.HasFlag(OrderIncludes.KitchenTickets))
            AddInclude(o => o.KitchenTickets);

        // Nested Include (User → Addresses)
        if (includes.HasFlag(OrderIncludes.UserAddresses))
        {
            AddInclude(o => o.User);
            AddInclude(o => o.User!.Addresses);
        }
    }
}
