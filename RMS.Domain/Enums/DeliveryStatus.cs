using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Enums
{
    public enum DeliveryStatus : byte
    {
        UnAssigned,
        Assigned,
        PickedUp,
        OnTheWay,
        Delivered
    }
}
