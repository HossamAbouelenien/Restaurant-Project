using AutoMapper;
using RMS.Domain.Entities.CustomerBasket;
using RMS.Shared.DTOs.BasketDTOs;

namespace RMS.Services.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

        }
    }
}
