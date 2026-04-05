using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    internal class OrderProfile :Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDTO,Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer!.Name))
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.OrderType, opt => opt.MapFrom(src => src.OrderType.ToString()))
                .ReverseMap();


            CreateMap<OrderItemDTO, OrderItem>().ReverseMap();

            CreateMap<Order, OrderDetailsDTO>()
                    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer!.Name))
                    .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.Name))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                    .ForMember(dest => dest.OrderType, opt => opt.MapFrom(src => src.OrderType.ToString()))
                    .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment))
                    .ForMember(dest => dest.Delivery, opt => opt.MapFrom(src => src.Delivery))
                    .ForMember(dest => dest.KitchenTickets, opt => opt.MapFrom(src => src.KitchenTickets))
                    .ReverseMap();

            CreateMap<Payment, OrderPaymentDTO>().ReverseMap();
            CreateMap<Delivery, OrderDeliveryDTO>()
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver!.Name))
                .ReverseMap();
            CreateMap<KitchenTicket, OrderKitchenTicketsDTO>().ReverseMap();
        }
    }
}
