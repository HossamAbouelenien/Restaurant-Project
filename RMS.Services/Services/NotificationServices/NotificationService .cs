using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Exceptions;
using RMS.Services.Specifications.NotificationSpec;
using RMS.ServicesAbstraction.IHubServices.INotificationServices;
using RMS.Shared.DTOs.NotificationDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Services.Services.NotificationServices
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

        public async Task CreateNotification(Notification sentnotification, string groupName, string eventName)
        {

            var repo = _unitOfWork.GetRepository<Notification>();
            var notification = sentnotification;

            await _realTimeNotifier.NotifyAdmins(new
            {
                notification.Id,
                notification.Title,
                notification.Message,
                notification.BranchId,
                notification.CreatedAt
            }, groupName, eventName
            );


            await repo.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
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
            {
                throw new NotificationNotFoundException(id);
            }
                

            notification.IsRead = true;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
