using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.IUserServices;
using RMS.Shared;
using RMS.Shared.DTOs.AddressDTOs;
using RMS.Shared.DTOs.UserDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using System.Security.Claims;

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

        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAllUserWithBranchAsync([FromQuery] UserQueryParams queryParams)
        {
            var Users = await _userService.GetAllUserWithBranchAsync(queryParams);
            return Ok(Users);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ActionName("GetUserDetails")]
        public async Task<ActionResult<UserDetailsDTO>> GetUserDetailsAsync(string id)
        {
            var User = await _userService.GetUserDetailsAsync(id);
            return Ok(User);
        }


        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<UserDetailsDTO>> AddUserAsync(CreateUserDto createUserDto)
        {
            var user = await _userService.AddUserAsync(createUserDto);

            return CreatedAtAction("GetUserDetails", new { id = user.Id }, user);
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDetailsDTO>> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            if (id != updateUserDto.Id)
                return BadRequest("Id mismatch");

            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(user);
        }


        [Authorize]
        [HttpPatch("{id}/toggle-status")]
        public async Task<ActionResult> ToggleUserStatus(string id)
        {
            var result = await _userService.ToggleUserStatusAsync(id);
            return Ok(result);
        }


        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("Roles")]
        public async Task<ActionResult<List<string>>> GetRolesAsync()
        {
            var roles = await _userService.GetRolesAsync();
            return Ok(roles);
   
        }


        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("inactive")]
        public async Task<ActionResult<PaginatedResult<GetUserDTO>>> GetInactiveUsers([FromQuery] UserQueryParams queryParams)
        {
            var users = await _userService.GetInactiveUsersAsync(queryParams);
            return Ok(users);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost("AddCustomerAsync")]
        public async Task<ActionResult<CreateCustomerDTO>> AddCustomerAsync(CreateCustomerDTO createCustomerDTO)
        {
            var user = await _userService.AddCustomerAsync(createCustomerDTO);

            return Ok(user);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet("GetAllCustomerUserAysnc")]
        public async Task<ActionResult<PaginatedResult<GetCustomerDTO>>> GetAllCustomerUserAysnc([FromQuery]CustomerQueryParams queryParams)
        {
            var Users = await _userService.GetAllCustomerUserAysnc(queryParams);
            return Ok(Users);
        }

        [Authorize(Roles = SD.Role_Admin + " " + SD.Role_Customer)]
        [HttpPut("{id}/address")]
        public async Task<ActionResult<GetCustomerDTO>> UpdateCustomerAddress(string id, UpdateCustomerAddressDTO updateCustomerAddressDTO)
        {
            var result = await _userService.UpdateCustomerAddress(id, updateCustomerAddressDTO);
            return Ok(result);
        }


        [Authorize(Roles = SD.Role_Admin + " " + SD.Role_Customer)]
        [HttpPut("{userId}/addresses")]
        public async Task<IActionResult> UpdateAddress(
            [FromRoute] string userId,
            [FromBody] UpdateAddressDto dto)
        {
            

           
                await _userService.UpdateAddressAsync(userId, dto);

                return Ok(new
                {
                    message = "Address updated successfully"
                });
            
           
        }

        [Authorize(Roles = SD.Role_Admin + " " + SD.Role_Customer)]
        [HttpDelete("{userId}/addresses")]
        public async Task<IActionResult> DeleteAddress(
            [FromRoute] string userId,
            [FromBody] DeleteAddressDto dto)
        {
            

           
                await _userService.DeleteAddressAsync(userId, dto);

                return Ok(new
                {
                    message = "Address deleted successfully"
                });
            
           
        }

        [Authorize(Roles = SD.Role_Admin + " " + SD.Role_Customer)]
        [HttpGet("my-addresses")]
        public async Task<IActionResult> GetMyAddresses([FromQuery] AddressQueryParams queryParams)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            queryParams.UserId = userId;

            var result = await _userService.GetUserAddressesAsync(queryParams);

            return Ok(result);
        }

    }    

}
