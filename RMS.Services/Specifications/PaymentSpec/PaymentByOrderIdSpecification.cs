using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.PaymentSpec
{
    public class PaymentByOrderIdSpecification : BaseSpecifications<Payment>
    {
        public PaymentByOrderIdSpecification(int orderId)
            : base(p => p.OrderId == orderId)
        {
        }
    }
}
