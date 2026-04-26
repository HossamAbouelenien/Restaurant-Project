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
            

            CreateMap<UpdateAddressDto, Address>()
                .ForMember(d => d.BuildingNumber, o => o.MapFrom(s => s.BuildingNumber))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Street))
                .ForMember(d => d.City, o => o.MapFrom(s => s.City));
        }
    }
}
