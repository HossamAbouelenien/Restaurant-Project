using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.RecipeDTOs;

namespace RMS.Services.MappingProfiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipesListDTO>()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient!.Name))
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem!.Name))
                .ForMember(dest => dest.MenuItemArabicName, opt => opt.MapFrom(src => src.MenuItem!.ArabicName))
                .ForMember(dest => dest.IngredientArabicName, opt => opt.MapFrom(src => src.Ingredient!.ArabicName))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Ingredient!.Unit))

                .ReverseMap();

            CreateMap<AddRecipeToMenuItemDTO, Recipe>().ReverseMap();
        }
    }
}
