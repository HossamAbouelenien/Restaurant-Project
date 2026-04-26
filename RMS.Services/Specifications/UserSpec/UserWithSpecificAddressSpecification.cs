using RMS.Domain.Entities;
using RMS.Shared.DTOs.AddressDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.UserSpec
{
    public class UserWithSpecificAddressSpecification : BaseSpecifications<User>
    {
        public UserWithSpecificAddressSpecification(string userId, UpdateAddressDto dto)
            : base(u => u.Id == userId &&
                        u.Addresses.Any(a =>
                            a.BuildingNumber == dto.OldBuildingNumber &&
                            a.Street == dto.OldStreet &&
                            a.City == dto.OldCity))
        {
            AddInclude(u => u.Addresses);
        }
    }
}
