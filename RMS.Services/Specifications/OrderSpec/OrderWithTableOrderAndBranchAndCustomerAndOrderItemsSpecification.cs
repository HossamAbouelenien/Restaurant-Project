using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class OrderWithTableOrderAndBranchAndCustomerAndOrderItemsSpecification : BaseSpecifications<Order>
    {
        public OrderWithTableOrderAndBranchAndCustomerAndOrderItemsSpecification(int orderId) 
            : base(o => o.Id == orderId)
        {
            AddInclude(o => o.Customer!);
            AddInclude(o => o.Branch!);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.TableOrder!);
            AddInclude(o => o.Delivery!);
            AddInclude(o => o.Payment!);

        }
    
    }
}
