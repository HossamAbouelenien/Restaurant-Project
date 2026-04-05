using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.KitchenDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    public class kitchenProfile :Profile
    {
        public kitchenProfile()
        {
            CreateMap<KitchenTicket, KitchenTicketDTO>();

            CreateMap<KitchenTicket, KitchenTicketDetailsDto>()
                    .ForMember(dest => dest.Items,opt => opt.MapFrom(src =>src.Order!.OrderItems
                    .Select(i => $"{i.MenuItem!.Name} x{i.Quantity}") ));

            CreateMap<KitchenTicket, ActivePendingStationsDTOs>()
                    .ForMember(dest => dest.Station,opt => opt.MapFrom(src => src.Station))
                    .ForMember(dest => dest.PendingCount,opt => opt.Ignore());
        


        }
        
    }
}
