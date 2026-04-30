using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Exceptions;
using RMS.Services.Specifications.DeliverySpec;
using RMS.ServicesAbstraction.IServices.IDeliveryServices;
using RMS.ServicesAbstraction.IServices.IHubServices.INotificationServices;
using RMS.ServicesAbstraction.IServices.IHubServices.IRestaurantNotifier;
using RMS.Shared;
using RMS.Shared.DTOs.DeliveryDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using RMS.Shared.SharedResources;
using System.Security.Claims;

namespace RMS.Services.Services.DeliveryServices
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IRestaurantNotifier _restaurantNotifier;
        private readonly INotificationService _notificationService;

        public DeliveryService(IUnitOfWork unitOfWork, IMapper mapper,
            IHttpContextAccessor httpContextAccessor,UserManager<User> userManager
            ,IRestaurantNotifier restaurantNotifier , INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
           _restaurantNotifier = restaurantNotifier;
           _notificationService = notificationService;
        }


        public async Task<PaginatedResult<DeliveryDetailsDto>> GetAllDeliveriesAsync(DeliveryQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Delivery>();
            var spec = new DeliveriesWithOrderSpecification(queryParams);
            var deliveries = await repo.GetAllAsync(spec);
            var Data = _mapper.Map<IEnumerable<Delivery>, IEnumerable<DeliveryDetailsDto>>(deliveries);
            var ProductCount = Data.Count();
            var CountSpec = new DeliveriesCountSpecifications(queryParams);
            var TotalCount = await repo.CountAsync(CountSpec);

            return new PaginatedResult<DeliveryDetailsDto>(
                queryParams.PageIndex,
                ProductCount,
                TotalCount,
                Data
            );

        }

        public async Task<IEnumerable<DeliveryDetailsDto>> GetOwnAssignedDeliveriesAsync()
        {
            var driverId = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(driverId))
            {
                throw new UnauthorizedDriverException();
            }

            var spec = new DeliveriesWithOrderSpecification(driverId);
            var deliveries = await _unitOfWork.GetRepository<Delivery>().GetAllAsync(spec);
            var data = _mapper.Map<IEnumerable<DeliveryDetailsDto>>(deliveries);
            return data;
        }

        public async Task<DeliveryDetailsDto> GetDeliveryByIdAsync(int id)
        {
            var Repo = _unitOfWork.GetRepository<Delivery>();
            var spec = new DeliveryByIdSpecification(id);
            var delivery = await Repo.GetByIdAsync(spec);

            if (delivery is null)
            {
                throw new DeliveryNotFoundException(id);
            }

            var data = _mapper.Map<DeliveryDetailsDto>(delivery);
            return data;

        }


        public async Task<DeliveryDetailsDto> AssignDriverAsync(AssignDeliveryDto dto)
        {
            var orderRepo = _unitOfWork.GetRepository<Order>();
            var deliveryRepo = _unitOfWork.GetRepository<Delivery>();

            var orderSpec = new OrderByIdSpecification(dto.OrderId);
            var order = await orderRepo.GetByIdAsync(orderSpec);

            if (order == null)
                throw new OrderNotFoundException(dto.OrderId);

            if (order.OrderType != OrderType.Delivery)
                throw new InvalidOrderTypeException(order.OrderType.ToString());

            if (order.Delivery != null && order.Delivery.DriverId != null)
                throw new OrderAlreadyAssignedException(dto.OrderId);

            var driver = await _userManager.FindByIdAsync(dto.DriverId);

            if (driver == null)
                throw new DriverNotFoundException(dto.DriverId);

            if (!await _userManager.IsInRoleAsync(driver, SD.Role_Driver))
                throw new InvalidDriverRoleException(dto.DriverId);


            var deliverySpec = new DeliveryByOrderIdSpecification(dto.OrderId);
            var delivery = await deliveryRepo.GetByIdAsync(deliverySpec);

            if (delivery == null)
                throw new DeliveryNotFoundException(dto.OrderId);

            delivery.DriverId = dto.DriverId;
            //delivery.AssignedAt = DateTime.UtcNow;
            delivery.CreatedAt = DateTime.Now;

            delivery.DeliveryStatus = DeliveryStatus.Assigned;

            deliveryRepo.Update(delivery);

            await _unitOfWork.SaveChangesAsync();

           


            var spec = new DeliveryByIdSpecification(delivery.Id);
            var createdDelivery = await deliveryRepo.GetByIdAsync(spec);

            var result =  _mapper.Map<DeliveryDetailsDto>(createdDelivery);
            await _restaurantNotifier.SendAsync(
                "OrderAssignedToDriver",
                result,
                $"drivers_id_{dto.DriverId}",
                "admins",
                $"customers_id_{createdDelivery?.Order!.UserId}");


            await _notificationService.CreateNotification(
                        new Notification
                        {
                            Title = SharedResourcesKeys.OrderAssignedToYou,
                            Message = $"Delivery With {delivery.Id} is Assigned To You At {delivery.DeliveryAddress.BuildingNumber} - {delivery.DeliveryAddress.City}",
                            Type = "Assignation",
                            Role = SD.Role_Driver,
                            UserId = delivery.DriverId
                        },
                        $"drivers_id_{dto.DriverId}",
                        "OrderAssignedToDriver"
                    );


            return result;
        }



        public async Task<DeliveryDetailsDto> UpdateDeliveryStatusAsync(int id, UpdateDeliveryStatusDto dto, string userId, bool isAdmin)
        {
            var deliveryRepo = _unitOfWork.GetRepository<Delivery>();
            var orderRepo = _unitOfWork.GetRepository<Order>();

            
            var spec = new DeliveryByIdSpecification(id);
            var delivery = await deliveryRepo.GetByIdAsync(spec);

            if (delivery == null)
                throw new DeliveryNotFoundException(id);

            var order = await orderRepo.GetByIdAsync(delivery.OrderId);

            if (order == null)
                throw new OrderNotFoundException(delivery.OrderId);

            if (order.Status != OrderStatus.Ready)
            {
                throw new OrderNotReadyException(delivery.OrderId);
            }


            if (!isAdmin && delivery.DriverId != userId)
                throw new UnauthorizedDriverException();


            if (!Enum.TryParse<DeliveryStatus>(dto.Status, true, out var parsedStatus))
                throw new InvalidStatusValueException(dto.Status);


            if (!IsValidTransition(delivery.DeliveryStatus, parsedStatus))
                throw new InvalidStatusTransitionException(
                    delivery.DeliveryStatus.ToString(),
                    parsedStatus.ToString());


            if (parsedStatus == DeliveryStatus.Delivered)
            {
                delivery.DeliveredAt = DateTime.UtcNow;

                if (dto.CashCollected.HasValue)
                    delivery.CashCollected = dto.CashCollected;

               
                



                order.Status = OrderStatus.Delivered; 

                //orderRepo.Update(order);


                //delivery.DriverId = null;
            }

            


            delivery.DeliveryStatus = parsedStatus;

            deliveryRepo.Update(delivery);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.CreateNotification(
                        new Notification
                        {
                            Title = "Order Update",
                            Message = $"your order is {delivery.DeliveryStatus}",
                            Type = "DeliveryStatus",
                            Role = SD.Role_Customer,
                            UserId = delivery.Order!.UserId
                        },
                        $"customers_id_{delivery.Order!.UserId}",
                        "DeliveryStatusChange"
                    );



            var result = _mapper.Map<DeliveryDetailsDto>(delivery);
         
            await _restaurantNotifier.SendAsync(
                "deliveryUpdated",
                result,
                $"admins",
                $"customers_id_{delivery.Order!.UserId}");

            return result;



        }



        private bool IsValidTransition(DeliveryStatus current, DeliveryStatus next)
        {
            return (current, next) switch
            {
                (DeliveryStatus.Assigned, DeliveryStatus.PickedUp) => true,
                (DeliveryStatus.PickedUp, DeliveryStatus.OnTheWay) => true,
                (DeliveryStatus.OnTheWay, DeliveryStatus.Delivered) => true,
                _ => false
            };
        }



        
            public async Task<List<UnAssignDeliveryDto>> GetUnAssignedDeliveriesAsync()
            {
                var repo = _unitOfWork.GetRepository<Delivery>();
                var spec = new UnAssignedDeliveriesSpecification();

                var deliveries = await repo.GetAllAsync(spec);
                deliveries = deliveries.Where(e => e.Order!.Status != OrderStatus.Delivered);

                 if (deliveries == null || !deliveries.Any())
                     return new List<UnAssignDeliveryDto>();

               var data = _mapper.Map<List<UnAssignDeliveryDto>>(deliveries);
                return data;
            }


        public async Task<PaginatedResult<AvailableDriverDto>> GetAvailableDriversAsync(AvailableDriversQueryParams query)
        {
            var repo = _unitOfWork.GetRepository<User>();

            var spec = new AvailableDriversSpecification(query);

            var drivers = await repo.GetAllAsync(spec);

            var countSpec = new AvailableDriversSpecification(query);
            var count = await repo.CountAsync(countSpec);

            var data = _mapper.Map<List<AvailableDriverDto>>(drivers);

            return new PaginatedResult<AvailableDriverDto>(
                query.PageIndex,
                query.PageSize,
                count,
                data
            );
        }



    }
}
