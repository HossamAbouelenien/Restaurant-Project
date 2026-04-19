using RMS.Domain.Entities;
using RMS.Shared.DTOs.NotificationDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IHubServices.INotificationServices
{
    public interface INotificationService
    {
        Task CreateNotification(Notification sentnotification,string groupName, string eventName);
        Task<IEnumerable<NotificationDTO>> GetAllAsync(NotificationQueryParams queryParams);

        Task MarkAsReadAsync(int id);
    }
}
