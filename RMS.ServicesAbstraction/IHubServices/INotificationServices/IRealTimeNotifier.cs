using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IHubServices.INotificationServices
{
    public interface IRealTimeNotifier
    {
        Task NotifyAdmins(object data, string groupName, string eventName);

    }
}
