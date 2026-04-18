using RMS.Shared.DTOs.NotificationDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.Notifications
{
    public interface INotificationService
    {
        Task CreateLowStockNotification(int branchId, string ingredientName, decimal quantity);
        Task<IEnumerable<NotificationDTO>> GetAllAsync(NotificationQueryParams queryParams);

        Task MarkAsReadAsync(int id);
    }
}
