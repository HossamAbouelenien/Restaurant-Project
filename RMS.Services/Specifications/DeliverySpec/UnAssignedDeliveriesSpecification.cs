using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.DeliverySpec
{
    public class UnAssignedDeliveriesSpecification : BaseSpecifications<Delivery>
    {
        public UnAssignedDeliveriesSpecification()
            : base(d => d.DriverId == null)
        {
            AddInclude(d => d.Order!);
            AddInclude(d => d.Order!.Branch!);
            AddInclude(d => d.Order!.OrderItems!);
            AddInclude(d => d.DeliveryAddress);
            AddInclude("Order.OrderItems.MenuItem");
            AddOrderByDescending(d => d.CreatedAt);
        }
    }
}
