using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.TableDTOs;

namespace RMS.Services.MappingProfiles
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            CreateMap<Table, TableDTO>();
            CreateMap<CreateTableDTO, Table>();
        }
    }
}