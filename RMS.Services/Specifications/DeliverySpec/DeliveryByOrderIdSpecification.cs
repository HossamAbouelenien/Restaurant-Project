using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.DeliverySpec
{
    public class DeliveryByOrderIdSpecification : BaseSpecifications<Delivery>
    {
        public DeliveryByOrderIdSpecification(int orderId)
            : base(d => d.OrderId == orderId)
        {
        }
    }
}
