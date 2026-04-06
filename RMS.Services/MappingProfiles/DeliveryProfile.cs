using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.DeliveryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    public class DeliveryProfile:Profile
    {
        public DeliveryProfile()
        {
            CreateMap<Delivery, DeliveryDto>()
                    .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver!.UserName))
                    .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Order!.BranchId))
                    .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Order!.Branch!.Name));
        }
    }
}
