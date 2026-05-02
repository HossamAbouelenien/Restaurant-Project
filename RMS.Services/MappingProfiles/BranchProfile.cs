using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.BranchDTOs;
using RMS.Shared.DTOs.BranchStockDTOs;

namespace RMS.Services.MappingProfiles
{
     public class BranchProfile : Profile
        
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDTO>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address != null ? src.Address.BuildingNumber : 0))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address != null ? src.Address.Street : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address != null ? src.Address.City : null))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Address != null ? src.Address.Note : null))
                .ForMember(dest => dest.SpecialMark, opt => opt.MapFrom(src => src.Address != null ? src.Address.SpecialMark : null));
            // Reverse mapping for updating Branch from BranchDTO

            CreateMap<UpdateBranchDTO, Branch>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City,
                    Note = src.Note,
                    SpecialMark = src.SpecialMark
                }));
            // Reverse mapping for creating Branch from BranchDTO
            CreateMap<CreateBranchDTO, Branch>()
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
               {
                   BuildingNumber = src.BuildingNumber,
                   Street = src.Street,
                   City = src.City,
                   Note = src.Note,
                   SpecialMark = src.SpecialMark
               }));

            CreateMap<Branch, GetBranchDTO>()
             .ForMember(dest => dest.UsersCount, opt => opt.MapFrom(src => src.Users.Count))
             .ForMember(dest => dest.TablesCount, opt => opt.MapFrom(src => src.Tables.Count))
             .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
             .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
             .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
             .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Address.Note))
             .ForMember(dest => dest.SpecialMark, opt => opt.MapFrom(src => src.Address.SpecialMark));


            CreateMap<BranchStock, BranchStockDTO>()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient!.Name))
                .ForMember(dest => dest.IngredientArabicName, opt => opt.MapFrom(src => src.Ingredient!.ArabicName))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Ingredient!.Unit))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.Name))
                .ForMember(dest => dest.BranchArabicName, opt => opt.MapFrom(src => src.Branch!.ArabicName));



        }

    }
}
