using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.TableDTOs
{
   public class CreateTableDTO
    {
         
      
        public int BranchId { get; set; }
        public string TableNumber { get; set; } = default!;
        public int Capacity { get; set; }
    }
}

