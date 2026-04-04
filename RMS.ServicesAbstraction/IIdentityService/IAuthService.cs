using RMS.Shared.DTOs.IdentityDTOs;
using RMS.Shared.DTOs.UserDTOs;

namespace RMS.ServicesAbstraction.IIdentityService
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO);

        Task<TokenDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<bool> IsEmailExistsAsync(string email);

        Task<TokenDTO?> RefreshAccessTokenAsync(RefreshTokenRequestDTO refreshTokenRequestDTO);

        Task<UserDTO?> GetCurrentUserAsync(string email);

        Task<UserDTO> UpdateCurrentUserAsync(string email, UpdateCurrentUserDTO dto);
    }
}