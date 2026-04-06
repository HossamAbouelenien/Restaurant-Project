using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.DeliverySpec
{
    public class OrderByIdSpecification : BaseSpecifications<Order>
    {
        public OrderByIdSpecification(int orderId)
            : base(o => o.Id == orderId)
        {
            AddInclude(o => o.Delivery!);
        }
    }
}
