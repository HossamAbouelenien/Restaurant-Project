using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.IngredientDTOs;

namespace RMS.Services.MappingProfiles
{
    internal class IngredientsProfile : Profile
    {
        public IngredientsProfile()
        {
            CreateMap<Ingredient, IngredientDTO>().ReverseMap();
            CreateMap<CreateIngredientDTO, Ingredient>().ReverseMap();


        }
    }
}
