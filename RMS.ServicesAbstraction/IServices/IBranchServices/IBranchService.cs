using RMS.Shared;
using RMS.Shared.DTOs.BranchDTOs;
using RMS.Shared.QueryParams;

namespace RMS.ServicesAbstraction.IServices.IBranchServices
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
