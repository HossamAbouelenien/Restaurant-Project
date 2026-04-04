using RMS.Domain.Entities;
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
        Task<IEnumerable<GetUserDTO>> GetAllUserWithBranchAsync(UserQueryParams queryParams);
        Task<UserDetailsDTO> GetUserDetailsAsync(string id);
        Task<UserDetailsDTO> AddUserAsync(CreateUserDto createUserDto);

        Task<UserDetailsDTO> UpdateUserAsync(string id, UpdateUserDto updateUserDto );
    }
}
