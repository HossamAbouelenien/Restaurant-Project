using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.PaymentSpec
{
    public class PaymentCountSpecifications : BaseSpecifications<Payment>
    {
        public PaymentCountSpecifications(PaymentQueryParams queryParams)
            : base(p =>
                (!queryParams.OrderId.HasValue || p.OrderId == queryParams.OrderId) &&
                (string.IsNullOrEmpty(queryParams.Method) ||
                    p.PaymentMethod == Enum.Parse<PaymentMethod>(queryParams.Method, true)) &&
                (string.IsNullOrEmpty(queryParams.Status) ||
                    p.PaymentStatus == Enum.Parse<PaymentStatus>(queryParams.Status, true)) &&
                (!queryParams.BranchId.HasValue || p.Order.BranchId == queryParams.BranchId)
            )
        {
        }
    }
}