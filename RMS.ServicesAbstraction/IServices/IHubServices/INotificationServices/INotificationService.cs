using RMS.Domain.Entities;
using RMS.Shared.DTOs.NotificationDTOs;
using RMS.Shared.QueryParams;

namespace RMS.ServicesAbstraction.IServices.IHubServices.INotificationServices
{
    public interface INotificationService
    {
        Task CreateNotification(Notification sentnotification,string groupName, string eventName);
        Task<IEnumerable<NotificationDTO>> GetAllAsync(NotificationQueryParams queryParams);

        Task MarkAsReadAsync(int id);
    }
}
