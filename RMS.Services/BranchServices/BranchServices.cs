using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.BranchDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.BranchServices
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
            var Branches =  await _unitOfWork.GetRepository<Branch>().GetAllAsync();
            return _mapper.Map<IEnumerable<BranchDTO>>(Branches);
        }

        public async Task<BranchDTO> GetBranchByIdAsync(int Id)
        {

            var Branch = await _unitOfWork.GetRepository<Branch>().GetByIdAsync(Id);
                        return _mapper.Map<BranchDTO>(Branch);
        }

        public Task<BranchDTO> UpdateBranchAsync(int Id, BranchDTO BranchDTO)
        {
            throw new NotImplementedException();
        }
        public Task<BranchDTO> CreateBranchAsync(BranchDTO BranchDTO)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBranchAsync(int Id)
        {
            throw new NotImplementedException();
        }

    }
}
