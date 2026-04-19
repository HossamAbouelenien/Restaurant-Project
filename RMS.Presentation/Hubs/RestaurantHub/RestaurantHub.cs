using Microsoft.AspNetCore.SignalR;
using RMS.Shared.DTOs.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Hubs.RestaurantHub
{
    public class RestaurantHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            var branchId = user?.FindFirst("branchId")?.Value;
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(branchId))
            {
                await base.OnConnectedAsync();
                return;
            }

            if (user!.IsInRole(SD.Role_Admin))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "admins");
            }

            if (user.IsInRole(SD.Role_Chef))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"kitchen_branch_{branchId}"
                );
            }

            if (user.IsInRole(SD.Role_Cashier))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"cashiers_branch_{branchId}"
                );
            }

            if (user.IsInRole(SD.Role_Waiter))
            {
                await Groups.AddToGroupAsync(
                    Context.ConnectionId,
                    $"waiters_branch_{branchId}"
                );
            }

            if (user.IsInRole(SD.Role_Driver))
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
