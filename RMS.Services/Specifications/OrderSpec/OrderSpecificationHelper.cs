using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Shared.QueryParams;
using System.Linq.Expressions;

namespace RMS.Services.Specifications.OrderSpec
{
    public static class OrderSpecificationHelper
    {
        public static Expression<Func<Order, bool>> GetOrderCriteria(OrderQueryParams queryParams)
        {

            var status = Enum.TryParse<OrderStatus>(queryParams.Status, true, out var s) ? s : (OrderStatus?)null;
            var orderType = Enum.TryParse<OrderType>(queryParams.OrderType, true, out var o) ? o : (OrderType?)null;
            var paymentStatus = Enum.TryParse<PaymentStatus>(queryParams.PaymentStatus, true, out var ps) ? ps : (PaymentStatus?)null;
            var paymentMethod = Enum.TryParse<PaymentMethod>(queryParams.PaymentMethod, true, out var pm) ? pm : (PaymentMethod?)null;

            return order =>
                (!queryParams.BranchId.HasValue || order.BranchId == queryParams.BranchId.Value) &&
                (!status.HasValue || order.Status == status.Value) &&
                (!orderType.HasValue || order.OrderType == orderType.Value) &&
                (!paymentStatus.HasValue || order.Payment!.PaymentStatus == paymentStatus.Value) &&
                (!paymentMethod.HasValue || order.Payment!.PaymentMethod == paymentMethod.Value) &&
                (!queryParams.Date.HasValue ||
                    (order.CreatedAt.Year == queryParams.Date.Value.Year &&
                     order.CreatedAt.Month == queryParams.Date.Value.Month &&
                     order.CreatedAt.Day == queryParams.Date.Value.Day));
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
                (o.UserId == customerId);
        }

        public static Expression<Func<Order, bool>> GetActiveOrderCriteria(OrderQueryParams queryParams, string customerId)
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
                     o.CreatedAt.Day == queryParams.Date.Value.Day)) &&
                (o.UserId == customerId)&&
                o.Status != OrderStatus.Delivered && o.Status != OrderStatus.AwaitingPayment && o.Status != OrderStatus.Cancelled ;
        }
    }
}
