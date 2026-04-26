using RMS.Domain.Entities;
using RMS.Shared;
using RMS.Shared.DTOs.AddressDTOs;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.UserDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IUserServices
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

    }
}
