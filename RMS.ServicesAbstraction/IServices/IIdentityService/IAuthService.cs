using RMS.Shared.DTOs.IdentityDTOs;
using System.Security.Claims;

namespace RMS.ServicesAbstraction.IServices.IIdentityService
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO);

        Task<TokenDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<bool> IsEmailExistsAsync(string email);

        Task<TokenDTO?> RefreshAccessTokenAsync(RefreshTokenRequestDTO refreshTokenRequestDTO);

        Task<UserDTO?> GetCurrentUserAsync(string email);

        Task<UserDTO> UpdateCurrentUserAsync(string email, UpdateCurrentUserDTO dto);

        Task<string> ConfirmEmailAsync(string? userId, string? code);
        Task<string> SendResetPasswordCode(string email);
        Task<(string result, string? resetSessionToken)> VerifyResetCode(string code);
        Task<string> ResetPassword(string resetSessionToken, string newPassword, string confirmPassword);
        Task<TokenDTO?> ExternalLoginAsync(ClaimsPrincipal principal, string provider);

    }
}