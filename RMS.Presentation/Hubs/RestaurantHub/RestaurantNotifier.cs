using Microsoft.AspNetCore.SignalR;
using RMS.ServicesAbstraction.IHubServices.IRestaurantNotifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Hubs.RestaurantHub
{
    public class RestaurantNotifier : IRestaurantNotifier
    {
        private readonly IHubContext<RestaurantHub> _hub;

        public RestaurantNotifier(IHubContext<RestaurantHub> hub)
        {
            _hub = hub;
        }

        public async Task SendAsync(string eventName, object data, params string[] groups)
        {
            foreach (var group in groups)
            {
                await _hub.Clients.Group(group)
                    .SendAsync(eventName, data);
            }
        }
    }
}
