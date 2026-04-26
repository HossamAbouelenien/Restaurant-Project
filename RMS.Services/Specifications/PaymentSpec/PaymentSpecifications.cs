using RMS.Domain.Entities;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.PaymentSpec
{
    public class PaymentSpecifications : BaseSpecifications<Payment>
    {
        public PaymentSpecifications(PaymentQueryParams queryParams)
            : base(p =>
                (!queryParams.OrderId.HasValue || p.OrderId == queryParams.OrderId) &&
                (string.IsNullOrEmpty(queryParams.Status) || p.PaymentStatus.ToString() == queryParams.Status) &&
                (string.IsNullOrEmpty(queryParams.Method) || p.PaymentMethod.ToString() == queryParams.Method)
            )
        {
            AddInclude(p => p.Order);
            AddInclude("Order.Branch");

            AddOrderByDescending(p => p.CreatedAt);

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
    }
}
