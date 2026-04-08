using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.RecipeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipesListDTO>()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient!.Name))
                .ForMember(dest => dest.MenuItemName, opt => opt.MapFrom(src => src.MenuItem!.Name))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Ingredient!.Unit))
                .ReverseMap();

            CreateMap<AddRecipeToMenuItemDTO, Recipe>().ReverseMap();
        }
    }
}
