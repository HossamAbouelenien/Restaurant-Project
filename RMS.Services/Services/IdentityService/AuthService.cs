using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Exceptions;
using RMS.Services.Exceptions.Base;
using RMS.Services.Specifications.IdentitySpec;
using RMS.ServicesAbstraction.IEmailServices;
using RMS.ServicesAbstraction.IIdentityService;
using RMS.Shared.DTOs.IdentityDTOs;
using RMS.Shared.SharedResources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace RMS.Services.Services.IdentityService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper,
             UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
             ITokenService tokenService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _emailService = emailService;

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

               
                if (!user.EmailConfirmed)
                {
                    throw new EmailNotConfirmedException(loginRequestDTO.Email);
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
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);

                await _tokenService.SaveRefreshTokenAsync(user.Id, jwtTokenId, newRefreshToken, refreshTokenExpiry);

                TokenDTO tokenDTO = new TokenDTO
                {
                    AccessToken = token,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = jwtToken.ValidTo,
                    RoleId = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Customer"
                };

                return tokenDTO;
            }


            catch (AppException)
            {
                
                throw;
            }


            catch (Exception ex)
            {
                throw new AuthException();
            }



        }







        public async Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (await IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    throw new EmailAlreadyExistsException(registerationRequestDTO.Email);
                }

                User user = new()
                {
                    Email = registerationRequestDTO.Email,
                    Name = registerationRequestDTO.Name,
                    UserName = registerationRequestDTO.Email,
                    NormalizedEmail = registerationRequestDTO.Email.ToUpper(),
                    
                    EmailConfirmed = false,
                    RoleId = string.IsNullOrEmpty(registerationRequestDTO.Role) ? "Customer" : registerationRequestDTO.Role
                };

                var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new RegistrationFailedException(errors);
                }

               
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var role = string.IsNullOrEmpty(registerationRequestDTO.Role) ? "Customer" : registerationRequestDTO.Role;

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(user, role);

                var userDto = _mapper.Map<UserDTO>(user);
                userDto.Role = role;
                userDto.ConfirmationToken = token; 


               
                var baseUrl = _configuration["URLs:BaseURL"];
                var confirmationLink = $"{baseUrl}api/Auth/confirm-email?userId={user.Id}&code={Uri.EscapeDataString(token)}";
                await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your email by clicking this link: {confirmationLink}");

                return userDto;
            }


            catch (AppException)
            {
                throw;
            }


            catch (Exception ex)
            {
                throw new AuthException();
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



            catch (AppException)
            {
                throw;
            }


            catch (Exception)
            {
                throw new AuthException();
            }




        }




        public async Task<UserDTO?> GetCurrentUserAsync(string email)
        {
            var spec = new UserByEmailSpecification(email);

            var user = await _unitOfWork.GetRepository<User>()
                .GetByIdAsync(spec);


            if (user == null)
            {
                throw new UserNotFoundException(email);
            }
               

            return _mapper.Map<UserDTO>(user);
        }




        public async Task<UserDTO> UpdateCurrentUserAsync(string email, UpdateCurrentUserDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
            {
                throw new UserNotFoundException(email);
            }
           
                       

            _mapper.Map(dto, user); 

            
            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, dto.NewPassword);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {

                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new RegistrationFailedException(errors);

            }

            var role = await _userManager.GetRolesAsync(user);

            return new UserDTO
            {
                Name = user.Name,
                Email = user.Email!,
                Role = role.FirstOrDefault() ?? "Customer"
            };
        }

        





        public async Task<string> ConfirmEmailAsync(string? userId, string? code)
        {
            if (userId == null || code == null)
                return SharedResourcesKeys.EmailIsNotConfirmed;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return SharedResourcesKeys.EmailIsNotConfirmed;

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
                return SharedResourcesKeys.ErrorHappend;

            return SharedResourcesKeys.Success;
        }

        




        public async Task<string> SendResetPasswordCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return SharedResourcesKeys.NotFound;

            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            var codeHash = BCrypt.Net.BCrypt.HashPassword(code);

            var session = new ResetSessionToken
            {
                UserId = user.Id,
                TokenHash = codeHash,
                ExpiryDate = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _unitOfWork.GetRepository<ResetSessionToken>().AddAsync(session);
            await _unitOfWork.SaveChangesAsync();

            await _emailService.SendEmailAsync(user.Email, $"Reset Code: {code}", "Reset Password");

            return SharedResourcesKeys.Success;
        }


        
        public async Task<(string result, string? resetSessionToken)> VerifyResetCode(string code)
        {
            var tokens = await _unitOfWork.GetRepository<ResetSessionToken>()
                .GetAllAsync();

            var session = tokens.FirstOrDefault(t =>
                !t.IsUsed &&
                t.ExpiryDate > DateTime.UtcNow &&
                BCrypt.Net.BCrypt.Verify(code, t.TokenHash));

            if (session == null)
                return ("InvalidOrExpiredCode", null);

            // generate secure session token
            var resetSessionToken = Guid.NewGuid().ToString();

            session.TokenHash = BCrypt.Net.BCrypt.HashPassword(resetSessionToken);

            _unitOfWork.GetRepository<ResetSessionToken>().Update(session);
            await _unitOfWork.SaveChangesAsync();

            return ("Valid", resetSessionToken);
        }

       


        public async Task<string> ResetPassword(string resetSessionToken, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
                return "PasswordsNotMatch";

            var tokens = await _unitOfWork.GetRepository<ResetSessionToken>()
                .GetAllAsync();

            var session = tokens.FirstOrDefault(t =>
                !t.IsUsed &&
                t.ExpiryDate > DateTime.UtcNow &&
                BCrypt.Net.BCrypt.Verify(resetSessionToken, t.TokenHash));

            if (session == null)
                return SharedResourcesKeys.Invalid;

            var user = await _userManager.FindByIdAsync(session.UserId);

            if (user == null)
                return SharedResourcesKeys.NotFound;

            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, newPassword);

            if (!result.Succeeded)
                return SharedResourcesKeys.Error;

            session.IsUsed = true;
            _unitOfWork.GetRepository<ResetSessionToken>().Update(session);
            await _unitOfWork.SaveChangesAsync();

            return SharedResourcesKeys.Success;
        }

       



        public async Task<TokenDTO?> ExternalLoginAsync(ClaimsPrincipal principal, string provider)
        {
            try
            {
                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                var name = principal.FindFirst(ClaimTypes.Name)?.Value;
                var providerId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(providerId))
                    return null;

                
                var repo = _unitOfWork.GetRepository<UserProvider>();
                var spec = new UserProviderByProviderSpec(provider, providerId);
                var userProvider = await repo.GetByIdAsync(spec);

                User user;

                if (userProvider != null)
                {
                    user = userProvider.User;
                }
                else
                {
                    user = null;

                    if (!string.IsNullOrEmpty(email))
                    {
                        user = await _userManager.FindByEmailAsync(email);
                    }

                    if (user == null)
                    {
                        user = new User
                        {
                            Email = email,
                            Name = name,
                            UserName = email ?? Guid.NewGuid().ToString(),
                            EmailConfirmed = true,
                            RoleId = "Customer"
                        };

                        var createResult = await _userManager.CreateAsync(user);

                        if (!createResult.Succeeded)
                        {
                            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                            throw new RegistrationFailedException(errors);
                        }


                        if (!await _roleManager.RoleExistsAsync("Customer"))
                            await _roleManager.CreateAsync(new IdentityRole("Customer"));

                        await _userManager.AddToRoleAsync(user, "Customer");


                    }

                    var newProvider = new UserProvider
                    {
                        Provider = provider,
                        ProviderId = providerId,
                        UserId = user.Id
                    };

                    await _unitOfWork.GetRepository<UserProvider>().AddAsync(newProvider);
                    await _unitOfWork.SaveChangesAsync();
                }

                var token = await _tokenService.GenerateJwtTokenAsync(user);

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                var refreshToken = await _tokenService.GenerateRefreshTokenAsync();
                var expiry = DateTime.UtcNow.AddDays(7);

                await _tokenService.SaveRefreshTokenAsync(user.Id, jti, refreshToken, expiry);

                return new TokenDTO
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = jwt.ValidTo.ToUniversalTime()
                };
            }



            catch (AppException)
            {
                throw;
            }


            catch (Exception)
            {
                throw new AuthException();
            }


        }


    }

}





















































