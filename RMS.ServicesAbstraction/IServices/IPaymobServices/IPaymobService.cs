namespace RMS.ServicesAbstraction.IServices.IPaymobServices
{
   public interface IPaymobService
    {
        Task<string> GetPaymentKeyAsync(decimal amount, int orderId);
        string BuildIframeUrl(string paymentToken);
    }
}

