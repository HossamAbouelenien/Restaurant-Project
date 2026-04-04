using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.DTOs.RecipeDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    public class MenuItemProfile : Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItem, MenuItemDTO>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category!.Name))
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom<MenuItemImagesUrlResolver>());

            CreateMap<MenuItem, MenuItemDetailsDTO>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category!.Name));

            CreateMap<Recipe, RecipeDTO>()
                .ForMember(dest => dest.IngredientName,
                    opt => opt.MapFrom(src => src.Ingredient!.Name))
                .ForMember(dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Ingredient!.Unit.ToString()));
        }
    }
}
