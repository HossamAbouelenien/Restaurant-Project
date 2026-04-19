using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.Enums
{
   
    [Flags]
    public enum OrderIncludes
    {
        None = 0,
        User = 1,
        Payment = 2,
        OrderItems = 4,
        Delivery = 8,
        TableOrder = 16,
        KitchenTickets = 32,
        UserAddresses = 64
    }
}
