using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.IdentityDTOs;
using RMS.Shared.DTOs.UserDTOs;

namespace RMS.Services.MappingProfiles
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<User, GetUserDTO>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.Name));

            CreateMap<User, UserDetailsDTO>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.Name))
                .ForMember(dest => dest.Orders,
                         opt => opt.MapFrom(src => src.Orders.Select(o => new
                         {
                             o.Id,
                             Status = o.Status.ToString(),
                             OrderType = o.OrderType.ToString(),
                             o.TotalAmount,
                             o.CreatedAt
                         })))

                .ForMember(dest => dest.Deliveries,
                         opt => opt.MapFrom(src => src.Deliveries.Select(d => new
                         {
                             d.Id,
                             DeliveryStatus = d.DeliveryStatus.ToString(),
                             d.DeliveredAt,
                             d.CashCollected,
                             d.DeliveryAddress
                         })));


            CreateMap<User, CreateUserDto>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Branch!.Id));

            CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>();

            CreateMap<UpdateCurrentUserDTO, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }

    }
    
}