using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class OrderWithBranchAndCustomerAndOrderItemsSpecification : BaseSpecifications<Order>
    {
        public OrderWithBranchAndCustomerAndOrderItemsSpecification(int orderId) 
            : base(o => o.Id == orderId)
        {
            AddInclude(o => o.User!);
            AddInclude(o => o.Branch!);
            AddInclude(o => o.OrderItems);
            AddInclude("OrderItems.MenuItem");
            //AddInclude("User.Role");

        }

        public OrderWithBranchAndCustomerAndOrderItemsSpecification(OrderQueryParams queryParams) 
            : base(OrderSpecificationHelper.GetOrderCriteria(queryParams))
        {
            AddInclude(o => o.User!);
            AddInclude(o => o.Branch!);
            AddInclude(o => o.OrderItems);
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
            AddInclude("OrderItems.MenuItem");
            AddInclude("TableOrder.Table");
            AddInclude(o => o.Payment!);
            //AddInclude("Customer.Role");

        }

        public OrderWithBranchAndCustomerAndOrderItemsSpecification(OrderQueryParams queryParams, string customerId)
            : base(OrderSpecificationHelper.GetOrderCriteria(queryParams, customerId))
        {
            AddInclude(o => o.User!);
            AddInclude(o => o.Branch!);
            AddInclude(o => o.OrderItems);
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
            AddInclude("OrderItems.MenuItem");
            AddInclude("TableOrder.Table");
            //AddInclude("Customer.Role");


        }

    }
}
