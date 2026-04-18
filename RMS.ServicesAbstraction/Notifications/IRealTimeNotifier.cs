using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.Notifications
{
    public interface IRealTimeNotifier
    {
        Task NotifyAdmins(object data);

    }
}
