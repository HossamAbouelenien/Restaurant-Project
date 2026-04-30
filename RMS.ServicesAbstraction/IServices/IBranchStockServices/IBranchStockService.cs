using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.QueryParams;

namespace RMS.ServicesAbstraction.IServices.IBranchStockServices
{
    public interface IBranchStockService
    {
        Task<IEnumerable<BranchStockDTO>> GetAllBranchStockAsync(BrandStockQueryParams queryParams);
        Task<BranchStockDTO> GetBranchStockAsync(int id);
        Task<BranchStockDTO> UpdateBranchStockAsync(int id, UpdateBranchStockDTO UpdateBranchStock);
    }
}
