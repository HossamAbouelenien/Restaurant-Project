using AutoMapper;
using Microsoft.AspNetCore.Http;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.Services.Specifications.DeliverySpec;
using RMS.Services.Specifications.OrderSpec;
using RMS.ServicesAbstraction.IDeliveryServices;
using RMS.Shared;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.DeliveryDTOs;
using RMS.Shared.DTOs.OrderDTOs;
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

        public DeliveryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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

    }
}
