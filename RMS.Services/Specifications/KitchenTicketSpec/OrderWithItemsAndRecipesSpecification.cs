using RMS.Domain.Entities;

namespace RMS.Services.Specifications.KitchenTicketSpec
{
    public class OrderWithItemsAndRecipesSpecification : BaseSpecifications<Order>
    {
        public OrderWithItemsAndRecipesSpecification(int orderId)
            : base(o => o.Id == orderId)
        {
            AddInclude("OrderItems");
            AddInclude("OrderItems.MenuItem");
            AddInclude("OrderItems.MenuItem.Recipes");
            AddInclude("OrderItems.MenuItem.Recipes.Ingredient");
        }
    }
}
