using RMS.Shared.DTOs.TableDTOs;
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
    }
}
