using RMS.Shared;
using RMS.Shared.DTOs.AddressDTOs;
using RMS.Shared.DTOs.UserDTOs;
using RMS.Shared.QueryParams;

namespace RMS.ServicesAbstraction.IServices.IUserServices
{
    public interface IUserService
    {
        Task<PaginatedResult<GetUserDTO>> GetAllUserWithBranchAsync(UserQueryParams queryParams);
        Task<UserDetailsDTO> GetUserDetailsAsync(string id);
        Task<UserDetailsDTO> AddUserAsync(CreateUserDto createUserDto);

        Task<UserDetailsDTO> UpdateUserAsync(string id, UpdateUserDto updateUserDto );
        Task<bool> ToggleUserStatusAsync(string id);
        Task<PaginatedResult<GetUserDTO>> GetInactiveUsersAsync(UserQueryParams queryParams);
        Task<List<string>> GetRolesAsync();
        Task<GetCustomerDTO> AddCustomerAsync(CreateCustomerDTO createCustomerDTO);
        Task<PaginatedResult<GetCustomerDTO>> GetAllCustomerUserAysnc(CustomerQueryParams queryParams);
        Task<GetCustomerDTO> UpdateCustomerAddress(string id, UpdateCustomerAddressDTO updateCustomerAddressDTO);
        Task UpdateAddressAsync(string userId, UpdateAddressDto dto);
        Task DeleteAddressAsync(string userId, DeleteAddressDto dto);
        Task<PaginatedResult<AddressDto>> GetUserAddressesAsync(AddressQueryParams queryParams);

    }
}
