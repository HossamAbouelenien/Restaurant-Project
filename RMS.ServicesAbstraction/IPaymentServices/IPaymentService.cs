using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IPaymentServices
{
    public interface IPaymentService
    {
        Task<string> PayOrderAsync(int orderId, string userId);
        Task HandleWebhookAsync(PaymobWebhookDto dto);
    }
}
