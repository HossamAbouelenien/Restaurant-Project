using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.OrderSpec
{
    public static class OrderSpecificationHelper
    {
        public static Expression<Func<Order, bool>> GetOrderCriteria(OrderQueryParams queryParams)
        {
            
            var status = Enum.TryParse<OrderStatus>(queryParams.Status, true, out var s) ? s : (OrderStatus?)null;
            var orderType = Enum.TryParse<OrderType>(queryParams.OrderType, true, out var o) ? o : (OrderType?)null;

            return o =>
                (!queryParams.BranchId.HasValue || o.BranchId == queryParams.BranchId.Value) &&
                (!status.HasValue || o.Status == status.Value) &&
                (!orderType.HasValue || o.OrderType == orderType.Value) &&
                (!queryParams.Date.HasValue ||
                    (o.CreatedAt.Year == queryParams.Date.Value.Year &&
                     o.CreatedAt.Month == queryParams.Date.Value.Month &&
                     o.CreatedAt.Day == queryParams.Date.Value.Day));
        }

        public static Expression<Func<Order, bool>> GetOrderCriteria(OrderQueryParams queryParams, string customerId)
        {
            var status = Enum.TryParse<OrderStatus>(queryParams.Status, true, out var s) ? s : (OrderStatus?)null;
            var orderType = Enum.TryParse<OrderType>(queryParams.OrderType, true, out var o) ? o : (OrderType?)null;

            return o =>
                (!queryParams.BranchId.HasValue || o.BranchId == queryParams.BranchId.Value) &&
                (!status.HasValue || o.Status == status.Value) &&
                (!orderType.HasValue || o.OrderType == orderType.Value) &&
                (!queryParams.Date.HasValue ||
                    (o.CreatedAt.Year == queryParams.Date.Value.Year &&
                     o.CreatedAt.Month == queryParams.Date.Value.Month &&
                     o.CreatedAt.Day == queryParams.Date.Value.Day))&& 
                (o.CustomerId == customerId);
        }
    }
}
