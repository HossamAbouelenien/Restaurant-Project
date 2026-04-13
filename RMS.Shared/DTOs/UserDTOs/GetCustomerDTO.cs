using RMS.Shared.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.UserDTOs
{
    public class GetCustomerDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
    }
}
