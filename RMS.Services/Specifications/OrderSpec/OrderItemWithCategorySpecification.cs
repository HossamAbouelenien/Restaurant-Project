using RMS.Domain.Entities;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class OrderItemWithCategorySpecification : BaseSpecifications<OrderItem>
    {
        public OrderItemWithCategorySpecification(int id) : base(o => o.Id == id)
        {
            AddInclude("MenuItem.Category");
        }
    }
}
