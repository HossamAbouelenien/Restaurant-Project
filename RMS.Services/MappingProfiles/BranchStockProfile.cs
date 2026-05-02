using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.BranchStockDTOs;

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
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.Name))
                .ForMember(dest => dest.BranchArabicName, opt => opt.MapFrom(src => src.Branch!.ArabicName));

            CreateMap<UpdateBranchStockDTO, BranchStock>();
        }
    }
}
