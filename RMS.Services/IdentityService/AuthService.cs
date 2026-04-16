using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction.IIdentityService;
using RMS.Shared.DTOs.IdentityDTOs;
using System.IdentityModel.Tokens.Jwt;

namespace RMS.Services.IdentityService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper,
             UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
             ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User is not null ? true : false;
        }

        public async Task<TokenDTO?> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);

                if (user == null || user.IsDeleted)
                {
                    return null;
                }

                bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (!isValid)
                {
                    return null;
                }

                var token = await _tokenService.GenerateJwtTokenAsync(user);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var jwtTokenId = jwtToken.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti)?.Value;

                var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();
                var refreshTokenExpiry = DateTime.Now.AddDays(7);

                await _tokenService.SaveRefreshTokenAsync(user.Id, jwtTokenId, newRefreshToken, refreshTokenExpiry);

                TokenDTO tokenDTO = new TokenDTO
                {
                    AccessToken = token,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = jwtToken.ValidTo
                };

                return tokenDTO;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred during user login", ex);
            }
        }

        public async Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (await IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    throw new InvalidOperationException($"User with email '{registerationRequestDTO.Email}' already exists");
                }

                User user = new()
                {
                    Email = registerationRequestDTO.Email,
                    Name = registerationRequestDTO.Name,
                    UserName = registerationRequestDTO.Email,
                    NormalizedEmail = registerationRequestDTO.Email.ToUpper(),
                    EmailConfirmed = true,
                    RoleId = string.IsNullOrEmpty(registerationRequestDTO.Role) ? "Customer" : registerationRequestDTO.Role
                };

                var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"User registration failed: {errors}");
                }

                var role = string.IsNullOrEmpty(registerationRequestDTO.Role) ? "Customer" : registerationRequestDTO.Role;

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(user, role);

                var userDto = _mapper.Map<UserDTO>(user);
                userDto.Role = role;

                return userDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred during user registration", ex);
            }
        }

        public async Task<TokenDTO?> RefreshAccessTokenAsync(RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshTokenRequestDTO.RefreshToken))
                {
                    return null;
                }

                var (isValid, userId, tokenFamilyId, tokenReused) = await _tokenService.ValidateRefreshTokenAsync(refreshTokenRequestDTO.RefreshToken);

                if (tokenReused)
                {
                    return null;
                }

                if (!isValid || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(tokenFamilyId))
                {
                    return null;
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return null;
                }

                await _tokenService.RevokeRefreshTokenAsync(refreshTokenRequestDTO.RefreshToken);

                var token = await _tokenService.GenerateJwtTokenAsync(user);

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();
                var refreshTokenExpiry = DateTime.UtcNow.AddMinutes(5);

                await _tokenService.SaveRefreshTokenAsync(user.Id, tokenFamilyId, newRefreshToken, refreshTokenExpiry);

                TokenDTO tokenDTO = new TokenDTO
                {
                    AccessToken = token,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = jwtToken.ValidTo
                };
                return tokenDTO;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred during token refresh", ex);
            }
        }

        public async Task<UserDTO?> GetCurrentUserAsync(string email)
        {
            var spec = new UserByEmailSpecification(email);

            var user = await _unitOfWork.GetRepository<User>()
                .GetByIdAsync(spec)
                ?? throw new KeyNotFoundException($"User with email {email} not found");

            return _mapper.Map<UserDTO>(user);
        }


        public async Task<UserDTO> UpdateCurrentUserAsync(string email, UpdateCurrentUserDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(email)
                       ?? throw new KeyNotFoundException();

            _mapper.Map(dto, user); 

            
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, dto.NewPassword);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var role = await _userManager.GetRolesAsync(user);

            return new UserDTO
            {
                Name = user.Name,
                Email = user.Email!,
                Role = role.FirstOrDefault() ?? "Customer"
            };
        }
    }
}