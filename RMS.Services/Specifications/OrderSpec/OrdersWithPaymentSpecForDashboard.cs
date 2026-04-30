using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.OrderSpec
{
    public class OrdersWithPaymentSpecForDashboard : BaseSpecifications<Order>
    {
        public OrdersWithPaymentSpecForDashboard()
            : base(o => !o.IsDeleted)
        {
            AddInclude(o => o.Payment!);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.Branch!);
        }
    }
}
