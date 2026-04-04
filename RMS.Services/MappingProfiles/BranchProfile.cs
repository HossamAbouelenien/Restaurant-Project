using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.BranchDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
     public class BranchProfile : Profile
        
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDTO>()
                 .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Address.Note))
                .ForMember(dest => dest.SpecialMark, opt => opt.MapFrom(src => src.Address.SpecialMark)); 
        }

    }
}
