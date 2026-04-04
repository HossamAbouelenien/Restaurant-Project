using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
    public interface IBranchStockService
    {
        Task<IEnumerable<BranchStockDTO>> GetAllBranchStockAsync(BrandStockQueryParams queryParams);
        Task<BranchStockDTO> GetBranchStockAsync(int id);
        Task<BranchStockDTO> UpdateBranchStockAsync(int id, UpdateBranchStockDTO UpdateBranchStock);
    }
}
