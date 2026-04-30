using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Exceptions;
using RMS.Services.Specifications.BranchSpec;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.BranchDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Services.Services.BranchServices
{
    public class BranchServices : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BranchServices( IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork= unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BranchDTO>> GetAllBranchesAsync()
        {
            var Branches = await _unitOfWork.GetRepository<Branch>().GetAllAsync();
            return _mapper.Map<IEnumerable<BranchDTO>>(Branches);
        }


        public async Task<PaginatedResult<GetBranchDTO>> GetAllBranchesWithOrdersAndTablesAsync(BranchQueryParams param)
        {
            var spec = new BranchWithDetailsSpecification(param);

            var branches = await _unitOfWork
                .GetRepository<Branch>()
                .GetAllAsync(spec);

            var countSpec = new BranchCountSpecification(param);

            var count = await _unitOfWork
                .GetRepository<Branch>()
                .CountAsync(countSpec);

            var data = _mapper.Map<IEnumerable<GetBranchDTO>>(branches);

            return new PaginatedResult<GetBranchDTO>(param.PageIndex, param.PageSize, count, data);
        }



        public async Task<GetBranchDTO> GetBranchByIdAsync(int id)
        {

            var spec = new BranchWithDetailsSpecification(id);

            var branch = await _unitOfWork
                .GetRepository<Branch>()
                .GetByIdAsync(spec);

            if(branch is null)
            {
                throw new BranchNotFoundException(id);
            }
                

            return _mapper.Map<GetBranchDTO>(branch);
        }




        public async Task<BranchDTO> UpdateBranchAsync(int Id, UpdateBranchDTO BranchDTO)
        {
            var repo = _unitOfWork.GetRepository<Branch>();
            var branch = await repo.GetByIdAsync(Id);
            if (branch is null)
                throw new BranchNotFoundException(Id);

            _mapper.Map(BranchDTO, branch);

            repo.Update(branch);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BranchDTO>(branch);
        }




        public async Task<BranchDTO> CreateBranchAsync(CreateBranchDTO BranchDTO)
        {
            var Repo = _unitOfWork.GetRepository<Branch>();
            var Branch = _mapper.Map<Branch>(BranchDTO);
            await Repo.AddAsync(Branch);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BranchDTO>(Branch);
        }




        public async Task DeleteBranchAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Branch>();
            var Branch = await repo.GetByIdAsync(id);

            if (Branch is null)
            {
                throw new BranchNotFoundException(id);
            }


            if (!Branch.IsActive)
            {
                throw new DeleteInActiveBranchException(id);
            }

            var orders = await _unitOfWork.GetRepository<Order>().GetAllAsync();
            var hasActiveOrders = orders.Any(o => o.BranchId == id &&
                                  o.Status != OrderStatus.Delivered &&
                                  o.Status != OrderStatus.Cancelled);

            if (hasActiveOrders)
            {
                throw new BranchHasActiveOrdersException(id);
            }

            Branch.IsDeleted = true;
            repo.Update(Branch);
            await _unitOfWork.SaveChangesAsync();
        }



        public async Task ToggleBranchStatusAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Branch>();
            var Branch = await repo.GetByIdAsync(id);

            if (Branch is null)
            {
                throw new BranchNotFoundException(id);
            }

            Branch.IsActive = !Branch.IsActive;
            repo.Update(Branch);
            await _unitOfWork.SaveChangesAsync();
        }





    }


}


























