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

        public async Task<BranchDTO> UpdateBranchAsync(int Id, UpdateBranchDTO BranchDTO)
        {
            var repo = _unitOfWork.GetRepository<Branch>();
            var branch = await repo.GetByIdAsync(Id);

            if (branch is null)
                return null;

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
            //var repo = _unitOfWork.GetRepository<Branch>();
            //var Branch = await repo.GetByIdAsync(id);
            //if (Branch is null)
            //    throw new Exception("Branch with id not found");
            //Branch!.IsDeleted = true; 
            //repo.Update(Branch);
            //await _unitOfWork.SaveChangesAsync();
            var repo = _unitOfWork.GetRepository<Branch>();
            var Branch = await repo.GetByIdAsync(id);

            if (Branch is null) return;

            Branch.IsDeleted = true;
            repo.Update(Branch);
            await _unitOfWork.SaveChangesAsync();
        }


    }
}
