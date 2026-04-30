using Microsoft.AspNetCore.SignalR;
using RMS.Shared.DTOs.Utility;
using System.Security.Claims;

namespace RMS.Presentation.Hubs.Notification
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {

            var user = Context.User;

            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (Context.User?.IsInRole(SD.Role_Admin) == true)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, SD.Group_Admins);
            }

            if (user!.IsInRole(SD.Role_Driver))
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                        $"drivers_id_{userId}"
                    );
                }
            }

            if (user.IsInRole(SD.Role_Customer))
            {

                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.AddToGroupAsync(
                        Context.ConnectionId,
                        $"customers_id_{userId}"
                    );
                }
            }

            await base.OnConnectedAsync();
        }
    }
}
