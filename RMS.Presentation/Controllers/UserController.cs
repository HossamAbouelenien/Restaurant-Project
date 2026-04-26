using Microsoft.AspNetCore.Mvc;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IUserServices;
using RMS.Shared;
using RMS.Shared.DTOs.AddressDTOs;
using RMS.Shared.DTOs.UserDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAllUserWithBranchAsync([FromQuery] UserQueryParams queryParams)
        {
            var Users = await _userService.GetAllUserWithBranchAsync(queryParams);
            return Ok(Users);
        }

        [HttpGet("{id}")]
        [ActionName("GetUserDetails")]
        public async Task<ActionResult<UserDetailsDTO>> GetUserDetailsAsync(string id)
        {
            var User = await _userService.GetUserDetailsAsync(id);
            return Ok(User);
        }


        [HttpPost]
        public async Task<ActionResult<UserDetailsDTO>> AddUserAsync(CreateUserDto createUserDto)
        {
            var user = await _userService.AddUserAsync(createUserDto);

            return CreatedAtAction("GetUserDetails", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDetailsDTO>> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            if (id != updateUserDto.Id)
                return BadRequest("Id mismatch");

            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(user);
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<ActionResult> ToggleUserStatus(string id)
        {
            var result = await _userService.ToggleUserStatusAsync(id);
            return Ok(result);
        }

        [HttpGet("Roles")]
        public async Task<ActionResult<List<string>>> GetRolesAsync()
        {
            var roles = await _userService.GetRolesAsync();
            return Ok(roles);
   
        }

        [HttpGet("inactive")]
        public async Task<ActionResult<PaginatedResult<GetUserDTO>>> GetInactiveUsers([FromQuery] UserQueryParams queryParams)
        {
            var users = await _userService.GetInactiveUsersAsync(queryParams);
            return Ok(users);
        }


        [HttpPost("AddCustomerAsync")]
        public async Task<ActionResult<CreateCustomerDTO>> AddCustomerAsync(CreateCustomerDTO createCustomerDTO)
        {
            var user = await _userService.AddCustomerAsync(createCustomerDTO);

            return Ok(user);
        }

        [HttpGet("GetAllCustomerUserAysnc")]
        public async Task<ActionResult<PaginatedResult<GetCustomerDTO>>> GetAllCustomerUserAysnc([FromQuery]CustomerQueryParams queryParams)
        {
            var Users = await _userService.GetAllCustomerUserAysnc(queryParams);
            return Ok(Users);
        }

        [HttpPut("{id}/address")]
        public async Task<ActionResult<GetCustomerDTO>> UpdateCustomerAddress(string id, UpdateCustomerAddressDTO updateCustomerAddressDTO)
        {
            var result = await _userService.UpdateCustomerAddress(id, updateCustomerAddressDTO);
            return Ok(result);
        }


        
        [HttpPut("{userId}/addresses")]
        public async Task<IActionResult> UpdateAddress(
            [FromRoute] string userId,
            [FromBody] UpdateAddressDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.UpdateAddressAsync(userId, dto);

                return Ok(new
                {
                    message = "Address updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        
        [HttpDelete("{userId}/addresses")]
        public async Task<IActionResult> DeleteAddress(
            [FromRoute] string userId,
            [FromBody] DeleteAddressDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.DeleteAddressAsync(userId, dto);

                return Ok(new
                {
                    message = "Address deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


    }    
}
