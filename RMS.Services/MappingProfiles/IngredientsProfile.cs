using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.IngredientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
