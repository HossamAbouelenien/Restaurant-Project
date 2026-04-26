using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.AddressDTOs
{
    public class UpdateAddressDto
    {
      
        public int OldBuildingNumber { get; set; }
        public string OldStreet { get; set; } = default!;
        public string OldCity { get; set; } = default!;

        public int BuildingNumber { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Note { get; set; }
        public string? SpecialMark { get; set; }
    }
}
