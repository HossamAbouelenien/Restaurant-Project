using Microsoft.AspNetCore.SignalR;
using RMS.Shared.DTOs.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            if (Context.User?.IsInRole(SD.Role_Admin) == true)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, SD.Group_Admins);
            }

            await base.OnConnectedAsync();
        }
    }
}
