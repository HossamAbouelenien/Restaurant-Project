using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.Utility
{
    public class PaymobSettings
    {
        public const string SectionName = "Paymob";

        public int IntegrationId { get; set; }
        public string? SecretKey { get; set; }
        public string? ApiKey { get; set; }
        public PaymobEndpoints? EndPoints { get; set; }
    }

    public class PaymobEndpoints
    {
        public string? AuthUrl { get; set; }
        public string? CreateOrderUrl { get; set; }
        public string? PaymentKeyUrl { get; set; }
        public string? IFrameUrl { get; set; }
    }
}
