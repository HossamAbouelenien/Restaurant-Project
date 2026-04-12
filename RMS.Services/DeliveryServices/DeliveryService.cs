using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.Services.Specifications.DeliverySpec;
using RMS.Services.Specifications.OrderSpec;
using RMS.ServicesAbstraction.IDeliveryServices;
using RMS.Shared;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.DeliveryDTOs;
using RMS.Shared.DTOs.OrderDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.DeliveryServices
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public DeliveryService(IUnitOfWork unitOfWork, IMapper mapper,
            IHttpContextAccessor httpContextAccessor,UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
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
                throw new Exception("Driver ID not found ");
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
            if (delivery == null)
            {
                throw new Exception("Delivery not found");
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
                throw new Exception("Order not found");

            if (order.OrderType != OrderType.Delivery)
                throw new Exception("Order is not a delivery type");

            if (order.Delivery != null && order.Delivery.DriverId != null)
                throw new Exception("Order already assigned");

            var driver = await _userManager.FindByIdAsync(dto.DriverId);

            if (driver == null)
                throw new Exception("Driver not found");

            if (!await _userManager.IsInRoleAsync(driver, SD.Role_Driver))
                throw new Exception("User is not a driver");

            
            var deliverySpec = new DeliveryByOrderIdSpecification(dto.OrderId);
            var delivery = await deliveryRepo.GetByIdAsync(deliverySpec);

            if (delivery == null)
                throw new Exception("Delivery not found");

            delivery.DriverId = dto.DriverId;
            //delivery.AssignedAt = DateTime.UtcNow;
            delivery.CreatedAt = DateTime.UtcNow;

            deliveryRepo.Update(delivery);
            await _unitOfWork.SaveChangesAsync();

            var spec = new DeliveryByIdSpecification(delivery.Id);
            var createdDelivery = await deliveryRepo.GetByIdAsync(spec);

            return _mapper.Map<DeliveryDetailsDto>(createdDelivery);
        }



        public async Task<DeliveryDetailsDto> UpdateDeliveryStatusAsync(int id, UpdateDeliveryStatusDto dto, string userId, bool isAdmin)
        {
            var deliveryRepo = _unitOfWork.GetRepository<Delivery>();
            var orderRepo = _unitOfWork.GetRepository<Order>();

            
            var spec = new DeliveryByIdSpecification(id);
            var delivery = await deliveryRepo.GetByIdAsync(spec);

            if (delivery == null)
                throw new Exception("Delivery not found");

           
            if (!isAdmin && delivery.DriverId != userId)
                throw new Exception("Unauthorized");

            
            if (!Enum.TryParse<DeliveryStatus>(dto.Status, true, out var parsedStatus))
                throw new Exception("Invalid status value");

            
            if (!IsValidTransition(delivery.DeliveryStatus, parsedStatus))
                throw new Exception("Invalid status transition");

            if (parsedStatus == DeliveryStatus.Delivered)
            {
                delivery.DeliveredAt = DateTime.UtcNow;

                if (dto.CashCollected.HasValue)
                    delivery.CashCollected = dto.CashCollected;

               
                var order = await orderRepo.GetByIdAsync(delivery.OrderId);

                if (order == null)
                    throw new Exception("Related order not found");

                order.Status = OrderStatus.Delivered; 

                orderRepo.Update(order);

                
                delivery.DriverId = null;
            }

            
            delivery.DeliveryStatus = parsedStatus;

            deliveryRepo.Update(delivery);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DeliveryDetailsDto>(delivery);
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
