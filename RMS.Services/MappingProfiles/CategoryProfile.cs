using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.CategoryDTOs;

namespace RMS.Services.MappingProfiles
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category,CategoryDTO>()
                .ForMember(dest => dest.MenuItemsCount,opt => opt.MapFrom(src => src.MenuItems.Count));


            CreateMap<CreateCategoryDTO, Category>();


        }



    }
}
