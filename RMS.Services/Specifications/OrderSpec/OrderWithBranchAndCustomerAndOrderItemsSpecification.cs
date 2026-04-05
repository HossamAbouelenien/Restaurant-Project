using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class OrderWithBranchAndCustomerAndOrderItemsSpecification : BaseSpecifications<Order>
    {
        public OrderWithBranchAndCustomerAndOrderItemsSpecification(int orderId) : base(o => o.Id == orderId)
        {
            AddInclude(o => o.Customer!);
            AddInclude(o => o.Branch!);
            AddInclude(o => o.OrderItems);
        }
    }
}
