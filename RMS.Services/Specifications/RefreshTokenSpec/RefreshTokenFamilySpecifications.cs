using RMS.Domain.Entities;

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