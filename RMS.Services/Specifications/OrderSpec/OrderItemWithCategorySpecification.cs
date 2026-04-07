using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
