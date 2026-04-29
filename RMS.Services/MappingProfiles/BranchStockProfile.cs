using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.BranchStockDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    public class BranchStockProfile :Profile
    {
        public BranchStockProfile()
        {
            CreateMap<BranchStock, BranchStockDTO>()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient!.Name))
                .ForMember(dest => dest.IngredientArabicName, opt => opt.MapFrom(src => src.Ingredient!.ArabicName))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Ingredient!.Unit))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.Name));

            CreateMap<UpdateBranchStockDTO, BranchStock>();
        }
    }
}
