using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.PaymentDTOs
{
  
        public record PaymentWebhookDto
        {
            public bool Success { get; init; }
            public string? MerchantOrderId { get; init; }
            public int Order { get; init; }
            public int AmountCents { get; init; }
            public string? Currency { get; init; }
            public bool IsCapture { get; init; }
            public bool ErrorOccurred { get; init; }
        }
        public record PayOrderWithCardResponse(
       string IFrameUrl,
        string Message);

    }

