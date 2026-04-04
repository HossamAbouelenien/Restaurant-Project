using RMS.Shared.DTOs.BranchDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
    public  interface IBranchService
    {
        Task<IEnumerable<BranchDTO>> GetAllBranchesAsync();
        Task<BranchDTO> GetBranchByIdAsync(int Id);
        Task<BranchDTO> CreateBranchAsync(BranchDTO BranchDTO);
         Task<BranchDTO> UpdateBranchAsync(int Id, BranchDTO BranchDTO);
         Task DeleteBranchAsync(int Id);
        



    }
}
