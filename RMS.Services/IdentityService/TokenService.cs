using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications;
using RMS.Services.Specifications.RefreshTokenSpec;
using RMS.ServicesAbstraction.IIdentityService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace RMS.Services.IdentityService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(
            IConfiguration configuration,
            UserManager<User> userManager,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtSettings")["Secret"]!);
            var roles = await _userManager.GetRolesAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email!),
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault()!),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("branchId", user.BranchId.ToString()!)
                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

           

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            var repo = _unitOfWork.GetRepository<RefreshToken>();
            var checkSpec = new RefreshTokenByValueSpecifications(refreshToken);
            var checkResult = await repo.GetByIdAsync(checkSpec);
            var exists = checkResult != null;

            if (exists)
            {
                return await GenerateRefreshTokenAsync();
            }

            return refreshToken;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshTokenId)
        {
            var repo = _unitOfWork.GetRepository<RefreshToken>();

            var storedTokenSpec = new RefreshTokenByValueSpecifications(refreshTokenId);
            var storedToken = await repo.GetByIdAsync(storedTokenSpec);

            if (storedToken == null)
            {
                return false;
            }
            storedToken.IsValid = false;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task SaveRefreshTokenAsync(string userId, string jwtTokenId, string refreshToken, DateTime expiresAt)
        {
            var refreshTokenEntity = new RefreshToken
            {
                UserId = userId,
                JwtTokenId = jwtTokenId,
                ExpiresAt = expiresAt,
                RefreshTokenValue = refreshToken,
                IsValid = true
            };
            await _unitOfWork.GetRepository<RefreshToken>().AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<(bool IsValid, string? UserId, string? TokenFamilyId, bool TokenReused)> ValidateRefreshTokenAsync(string refreshToken)
        {
            var repo = _unitOfWork.GetRepository<RefreshToken>();

            var storedTokenSpec = new RefreshTokenByValueSpecifications(refreshToken);
            var storedToken = await repo.GetByIdAsync(storedTokenSpec);

            if (storedToken == null)
            {
                return (false, null, null, false);
            }

            if (!storedToken.IsValid)
            {
                var familySpec = new RefreshTokenFamilySpecifications(storedToken.JwtTokenId, storedToken.UserId);
                var tokenFamily = await repo.GetAllAsync(familySpec);

                if (tokenFamily.Count() > 0)
                {
                    foreach (var token in tokenFamily)
                    {
                        token.IsValid = false;
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                return (false, storedToken.UserId, storedToken.JwtTokenId, true);
            }

            if (storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return (false, storedToken.UserId, storedToken.JwtTokenId, false);
            }

            return (true, storedToken.UserId, storedToken.JwtTokenId, false);
        }
    }
}