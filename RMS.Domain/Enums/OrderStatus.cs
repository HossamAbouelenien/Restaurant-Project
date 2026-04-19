using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Enums
{
    public enum OrderStatus : byte
    {
        Received,
        Preparing,
        Ready,
        Delivered,
        Cancelled,
        AwaitingPayment
    }
}
