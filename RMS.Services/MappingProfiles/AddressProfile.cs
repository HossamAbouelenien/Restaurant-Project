using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.AddressDTOs;

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
