using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.NotificationDTOs;

namespace RMS.Services.MappingProfiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDTO>()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : null));
        }
    }
}
