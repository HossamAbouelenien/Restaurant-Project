using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Enums
{
    public enum PaymentMethod : byte
    {
        Cash = 1,
        Card,
        InstaPay,
        Wallet
    }
}
