using RMS.Shared;
using RMS.Shared.DTOs.BranchDTOs;
using RMS.Shared.QueryParams;
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
        Task<PaginatedResult<GetBranchDTO>> GetAllBranchesWithOrdersAndTablesAsync(BranchQueryParams param);
        
        Task<GetBranchDTO> GetBranchByIdAsync(int id);
        Task<BranchDTO> CreateBranchAsync(CreateBranchDTO BranchDTO);
         Task<BranchDTO> UpdateBranchAsync(int Id, UpdateBranchDTO BranchDTO);
         Task DeleteBranchAsync(int Id);
        Task ToggleBranchStatusAsync(int id);




    }
}
