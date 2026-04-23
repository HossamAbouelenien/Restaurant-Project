using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.BranchDTOs
{
    public class CreateBranchDTO
    {
        public string Name { get; set; } = default!;
        public string ArabicName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Note { get; set; }
        public string? SpecialMark { get; set; }
    }
}
