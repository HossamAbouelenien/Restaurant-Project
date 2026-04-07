using RMS.Shared.DTOs.TableDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
   public interface ITableService
    {
        Task<TableDTO> CreateTableAsync(CreateTableDTO dto);
        Task<IEnumerable<TableDTO>> GetAllTablesAsync(TableQueryParams queryParams);
        Task<TableDTO> GetTableByIdAsync(int id);
        Task<TableDTO>UpdateTableAsync(int id,UpdateTableDTO dto);
        Task DeleteTableAsync(int id);
        Task<IEnumerable<TableOrderDTO>> GetAllTableOrdersAsync(TableOrderQueryParams queryParams);
        Task ToggleTableStatusAsync(int id);

    }
}
