using Microsoft.AspNetCore.SignalR;
using RMS.ServicesAbstraction.IServices.IHubServices.INotificationServices;

namespace RMS.Presentation.Hubs.Notification
{
    public class RealTimeNotifier : IRealTimeNotifier
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public RealTimeNotifier(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyAdmins(object data, string groupName, string eventName)
        {
            await _hubContext.Clients.Group(groupName)
                .SendAsync(eventName, data);
        }
    }
}
