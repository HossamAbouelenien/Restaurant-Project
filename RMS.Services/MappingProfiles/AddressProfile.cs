using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    public class AddressProfile :Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDto>();
        }
    }
}
