using AutoMapper;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.BranchDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
     public class BranchProfile : Profile
        
    {
        public BranchProfile()
        {
            CreateMap<Branch, BranchDTO>();
        }

    }
}
