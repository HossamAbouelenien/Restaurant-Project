using Microsoft.AspNetCore.SignalR;
using RMS.ServicesAbstraction.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Hubs
{
    public class RealTimeNotifier : IRealTimeNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public RealTimeNotifier(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAdmins(object data)
        {
            await _hubContext.Clients.Group("admins")
                .SendAsync("LowStockAlert", data);
        }
    }
}
