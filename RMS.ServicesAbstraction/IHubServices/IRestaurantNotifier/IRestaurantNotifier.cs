using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IHubServices.IRestaurantNotifier
{
    public interface IRestaurantNotifier
    {
        Task SendAsync(string eventName, object data, params string[] groups);

    }
}
