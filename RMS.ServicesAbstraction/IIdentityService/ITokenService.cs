using RMS.Domain.Entities;

namespace RMS.ServicesAbstraction.IIdentityService
{
    public interface ITokenService
    {
        Task<string> GenerateJwtTokenAsync(User user);

        Task<string> GenerateRefreshTokenAsync();

        Task SaveRefreshTokenAsync(string userId, string jwtTokenId, string refreshToken, DateTime expiresAt);

        Task<bool> RevokeRefreshTokenAsync(string refreshTokenId);

        Task<(bool IsValid, string? UserId, string? TokenFamilyId, bool TokenReused)> ValidateRefreshTokenAsync(string refreshToken);
    }
}