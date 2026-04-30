using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

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
