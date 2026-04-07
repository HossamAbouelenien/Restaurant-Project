using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.OrderSpec
{
    public class OrderCountSpecification : BaseSpecifications<Order>
    {
        public OrderCountSpecification(OrderQueryParams queryParams) 
            : base(OrderSpecificationHelper.GetOrderCriteria(queryParams))
        {
            
        }

        public OrderCountSpecification(OrderQueryParams queryParams, string customerId)
            : base(OrderSpecificationHelper.GetOrderCriteria(queryParams, customerId))
        {
            
        }
    }
}
