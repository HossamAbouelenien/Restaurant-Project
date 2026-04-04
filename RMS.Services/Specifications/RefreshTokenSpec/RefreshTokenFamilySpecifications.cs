using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.RefreshTokenSpec
{
    public class RefreshTokenFamilySpecifications : BaseSpecifications<RefreshToken>
    {
        public RefreshTokenFamilySpecifications(string jwtId, string userId)
        : base(u => u.JwtTokenId == jwtId && u.UserId == userId)
        {
        }
    }
}