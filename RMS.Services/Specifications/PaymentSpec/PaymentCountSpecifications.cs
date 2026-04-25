using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.PaymentSpec
{
    public class PaymentCountSpecifications : BaseSpecifications<Payment>
    {
        public PaymentCountSpecifications(PaymentQueryParams queryParams)
            : base(p =>
                (!queryParams.OrderId.HasValue || p.OrderId == queryParams.OrderId) &&
                (string.IsNullOrEmpty(queryParams.Status) || p.PaymentStatus.ToString() == queryParams.Status) &&
                (string.IsNullOrEmpty(queryParams.Method) || p.PaymentMethod.ToString() == queryParams.Method)
            )
        {
           
        }
    }
}
