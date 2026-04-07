using RMS.Domain.Entities;
using RMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.ReportSpec
{
    public class OrdersWithFullDataSpecification : BaseSpecifications<Order>
    {
        public OrdersWithFullDataSpecification(DateTime from, DateTime to)
            : base(o =>
                !o.IsDeleted &&
                o.Status != OrderStatus.Cancelled &&
                o.CreatedAt >= from &&
                o.CreatedAt < to)
        {
            AddInclude(o => o.OrderItems);
            AddInclude("OrderItems.MenuItem.Recipes");
            AddInclude("OrderItems.MenuItem.Recipes.Ingredient");
        }
    }
}
