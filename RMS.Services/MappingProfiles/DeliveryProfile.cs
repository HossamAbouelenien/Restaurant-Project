using AutoMapper;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Shared.DTOs.AddressDTOs;
using RMS.Shared.DTOs.DeliveryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    public class DeliveryProfile:Profile
    {
        public DeliveryProfile()
        {
           
            CreateMap<Order, OrderSummaryDto>()
                .ForMember(dest => dest.ItemsCount,opt => opt.MapFrom(src => src.OrderItems.Count))
                .ForMember(dest => dest.BranchName,opt => opt.MapFrom(src => src.Branch!.Name))
                .ForMember(dest => dest.OrderType,opt => opt.MapFrom(src => src.OrderType.ToString()))
                .ForMember(dest => dest.Status,opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
       


            CreateMap<Delivery, DeliveryDetailsDto>()
                .ForMember(dest => dest.DeliveryStatus,opt => opt.MapFrom(src => src.DeliveryStatus.ToString()))
                .ForMember(dest => dest.DriverName,opt => opt.MapFrom(src => src.Driver != null ? src.Driver.UserName : null))
                .ForMember(dest => dest.Order,opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.DeliveryAddress,opt => opt.MapFrom(src => src.DeliveryAddress));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.MenuItemName,opt => opt.MapFrom(src => src.MenuItem!.Name));

            CreateMap<AssignDeliveryDto, Delivery>()
                .ForMember(dest => dest.DeliveryAddress,opt => opt.MapFrom(src => src.DeliveryAddress))
                .ForMember(dest => dest.DeliveryStatus,opt => opt.MapFrom(src => DeliveryStatus.Assigned));
        


            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
