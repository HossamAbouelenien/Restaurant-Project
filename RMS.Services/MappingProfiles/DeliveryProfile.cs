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
                .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(src => src.DeliveryStatus.ToString()))
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver != null ? src.Driver.Name : null))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.DeliveryAddress, opt => opt.MapFrom(src => src.DeliveryAddress))
                .ForMember(dest => dest.CashCollected, opt => opt.MapFrom(src => src.CashCollected));
    

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.MenuItemName,opt => opt.MapFrom(src => src.MenuItem!.Name));

            CreateMap<AssignDeliveryDto, Delivery>()
                .ForMember(dest => dest.DeliveryAddress,opt => opt.MapFrom(src => src.DeliveryAddress))
                .ForMember(dest => dest.DeliveryStatus,opt => opt.MapFrom(src => DeliveryStatus.Assigned));


            CreateMap<Delivery, UnAssignDeliveryDto>()
                .ForMember(dest => dest.DeliveryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Order!.Id))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Order!.Branch!.Name))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Order!.User!.Name))
                .ForMember(dest => dest.ItemsCount, opt => opt.MapFrom(src => src.Order!.OrderItems.Count))
                .ForMember(dest => dest.DeliveryAddress, opt => opt.MapFrom(src => src.DeliveryAddress));

            CreateMap<User, AvailableDriverDto>()
                .ForMember(dest => dest.DriverId,opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PhoneNumber,opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.BranchId,opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.BranchName,opt => opt.MapFrom(src => src.Branch != null? src.Branch.Name: null));
                   
                   
               

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
