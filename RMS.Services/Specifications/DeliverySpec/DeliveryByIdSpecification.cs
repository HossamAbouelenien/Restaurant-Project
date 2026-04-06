using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.DeliverySpec
{
    public class DeliveryByIdSpecification : BaseSpecifications<Delivery>
    {
        public DeliveryByIdSpecification(int id)
            : base(d => d.Id == id)
        {
            AddInclude(d => d.Order!);
            AddInclude(d => d.Order!.Branch!);
            AddInclude(d => d.Order!.OrderItems!);
            AddInclude(d => d.Driver!);
            AddInclude(d => d.DeliveryAddress);
            AddInclude("Order.OrderItems.MenuItem");
        }
    }
}
