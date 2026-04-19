using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.NotificationSpec;
using RMS.ServicesAbstraction.Notifications;
using RMS.Shared.DTOs.NotificationDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRealTimeNotifier _realTimeNotifier;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IRealTimeNotifier realTimeNotifier, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _realTimeNotifier = realTimeNotifier;
            _mapper = mapper;
        }

        public async Task CreateLowStockNotification(int branchId, string ingredientName, decimal quantity)
        {
            var repo = _unitOfWork.GetRepository<Notification>();

            var notification = new Notification
            {
                Title = "Low Stock Alert",
                Message = $"{ingredientName} is low in branch {branchId}",
                BranchId = branchId,
                Type = "LowStock",
                Role = SD.Role_Admin,
            };

            await repo.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _realTimeNotifier.NotifyAdmins(new
            {
                notification.Id,
                notification.Title,
                notification.Message,
                notification.BranchId,
                notification.CreatedAt
            });
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllAsync(NotificationQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var spec = new NotificationWithBranchSpecification(queryParams);
            var notifications = await repo.GetAllAsync(spec);
            return _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
        }

        public async Task MarkAsReadAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Notification>();

            var notification = await repo.GetByIdAsync(id);

            if (notification == null)
                throw new Exception("Notification not found");

            notification.IsRead = true;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
