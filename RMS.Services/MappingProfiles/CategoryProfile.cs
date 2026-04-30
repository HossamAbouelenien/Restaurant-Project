using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.CategoryDTOs;

namespace RMS.Services.MappingProfiles
{
    internal class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.MenuItemsCount, opt => opt.MapFrom(src => src.MenuItems.Count));




            CreateMap<CreateCategoryDTO, Category>();


            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        }



    }
}
