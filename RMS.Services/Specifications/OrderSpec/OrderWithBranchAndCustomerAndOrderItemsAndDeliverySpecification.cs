using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class OrderWithBranchAndCustomerAndOrderItemsAndDeliverySpecification : BaseSpecifications<Order>
    {
        public OrderWithBranchAndCustomerAndOrderItemsAndDeliverySpecification(OrderQueryParams queryParams, string customerId)
            : base((OrderSpecificationHelper.GetActiveOrderCriteria(queryParams, customerId)))
        {
            AddInclude(o => o.User!);
            AddInclude(o => o.Branch!);
            AddInclude(o => o.OrderItems);
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
            //AddInclude("OrderItems.MenuItem");
            AddInclude("Delivery.Driver");
            //AddInclude("Customer.Role");
            AddOrderByDescending(o => o.CreatedAt);



        }
    }
}
