using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
   public interface IPaymobService
    {
        Task<string> GetPaymentKeyAsync(decimal amount, int orderId);
        string BuildIframeUrl(string paymentToken);
    }
}

