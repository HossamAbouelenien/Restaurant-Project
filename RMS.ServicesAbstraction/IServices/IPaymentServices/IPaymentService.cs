using RMS.Shared;
using RMS.Shared.DTOs.PaymentsDTOS;
using RMS.Shared.QueryParams;

namespace RMS.ServicesAbstraction.IServices.IPaymentServices
{
    public interface IPaymentService
    {
        Task<string> PayOrderAsync(int orderId, string userId);
        Task HandleWebhookAsync(PaymobWebhookDto dto);

        Task ConfirmCashPaymentAsync(int orderId , decimal paidAmount);
        Task<PaginatedResult<PaymentDto>> GetAllAsync(PaymentQueryParams queryParams);

        Task<List<PaymentDto>> GetAllWithoutPaginationAsync();
    }
}
