using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.IdentityDTOs;
using RMS.Shared.DTOs.TableDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.BranchDTOs
{
    public class GetBranchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string ArabicName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public bool IsActive { get; set; } = true;

        //Adress
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Note { get; set; }
        public string? SpecialMark { get; set; }

        public int UsersCount { get; set; }
        public int TablesCount { get; set; }

        public List<UserDTO> Users { get; set; } = new();
        public List<BranchStockDTO> BranchStocks { get; set; } = new();
        public List<TableDTO> Tables { get; set; } = new();
    }
}
