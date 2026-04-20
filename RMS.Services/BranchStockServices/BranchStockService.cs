using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications;
using RMS.Services.Specifications.BranchStockSpec;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IHubServices.IRestaurantNotifier;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.OrderDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.BranchStockServices
{
    public class BranchStockService : IBranchStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRestaurantNotifier _restaurantNotifier;

        public BranchStockService(IUnitOfWork unitOfWork, IMapper mapper , IRestaurantNotifier restaurantNotifier )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _restaurantNotifier = restaurantNotifier;
        }
        public async Task<IEnumerable<BranchStockDTO>> GetAllBranchStockAsync(BrandStockQueryParams queryParams)
        {
            var Repo = _unitOfWork.GetRepository<BranchStock>();
            var Spec = new BranchStockWithBranchAndIngredient(queryParams);
            var BranchStocks = await Repo.GetAllAsync(Spec);
            var DataToReturn = _mapper.Map<IEnumerable<BranchStockDTO>>(BranchStocks);
            return DataToReturn;
        }

        public async Task<BranchStockDTO> GetBranchStockAsync(int id)
        {
            var Repo = _unitOfWork.GetRepository<BranchStock>();
            var Spec = new BranchStockWithBranchAndIngredient(id);
            var BranchStock = await Repo.GetByIdAsync(Spec);
            var DataToReturn = _mapper.Map<BranchStockDTO>(BranchStock);
            return DataToReturn;
        }

        public async Task<BranchStockDTO> UpdateBranchStockAsync(int id, UpdateBranchStockDTO UpdateBranchStock)
        {
            var Repo = _unitOfWork.GetRepository<BranchStock>();
            var Spec = new BranchStockWithBranchAndIngredient(id);
            var BranchStock = await Repo.GetByIdAsync(Spec);

            _mapper.Map(UpdateBranchStock, BranchStock);

            BranchStock!.UpdatedAt = DateTime.Now;

            Repo.Update(BranchStock!);

            await _unitOfWork.SaveChangesAsync();

            var DataToReturn = _mapper.Map<BranchStockDTO>(BranchStock);

            await _restaurantNotifier.SendAsync(
                "BranchStockUpdated",
                DataToReturn,
                $"kitchen_branch_{DataToReturn.BranchId}",
                "admins");

            return DataToReturn;
        }
    }
}










































































































































// arwa