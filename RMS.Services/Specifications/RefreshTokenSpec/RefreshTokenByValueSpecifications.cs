using RMS.Domain.Entities;

namespace RMS.Services.Specifications.RefreshTokenSpec
{
    public class RefreshTokenByValueSpecifications : BaseSpecifications<RefreshToken>

    {
        public RefreshTokenByValueSpecifications(string token)
        : base(u => u.RefreshTokenValue == token)
        {
        }
    }
}