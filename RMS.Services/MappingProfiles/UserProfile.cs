using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.IdentityDTOs;

namespace RMS.Services.MappingProfiles
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}