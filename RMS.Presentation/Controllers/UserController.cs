using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAllUserWithBranchAsync([FromQuery] UserQueryParams queryParams)
        {
            _logger.LogInformation("GetAllUsers request started");
            var Users = await _userService.GetAllUserWithBranchAsync(queryParams);
            return Ok(Users);
        }

        //[Authorize]
        [HttpGet("{id}")]
        [ActionName("GetUserDetails")]
        public async Task<ActionResult<UserDetailsDTO>> GetUserDetailsAsync(string id)
        {
            _logger.LogInformation("GetUserDetails request started");
            var User = await _userService.GetUserDetailsAsync(id);
            return Ok(User);
        }


        //[Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<UserDetailsDTO>> AddUserAsync(CreateUserDto createUserDto)
        {
            _logger.LogInformation("AddUser request started");
            var user = await _userService.AddUserAsync(createUserDto);

            return CreatedAtAction("GetUserDetails", new { id = user.Id }, user);
        }


        //[Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDetailsDTO>> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            _logger.LogInformation("UpdateUser request started");
            if (id != updateUserDto.Id)
            {
                _logger.LogWarning("UpdateUser failed: id mismatch");
                return BadRequest("Id mismatch");

            }
                

            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(user);
        }


        //[Authorize]
        [HttpPatch("{id}/toggle-status")]
        public async Task<ActionResult> ToggleUserStatus(string id)
        {
            _logger.LogInformation("ToggleUserStatus request started");
            var result = await _userService.ToggleUserStatusAsync(id);
            return Ok(result);
        }


        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet("Roles")]
        public async Task<ActionResult<List<string>>> GetRolesAsync()
        {
            _logger.LogInformation("GetRoles request started");
            var roles = await _userService.GetRolesAsync();
            return Ok(roles);
   
        }


        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet("inactive")]
        public async Task<ActionResult<PaginatedResult<GetUserDTO>>> GetInactiveUsers([FromQuery] UserQueryParams queryParams)
        {
            _logger.LogInformation("GetInactiveUsers request started");
            var users = await _userService.GetInactiveUsersAsync(queryParams);
            return Ok(users);
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpPost("AddCustomerAsync")]
        public async Task<ActionResult<CreateCustomerDTO>> AddCustomerAsync(CreateCustomerDTO createCustomerDTO)
        {
            _logger.LogInformation("AddCustomer request started");
            var user = await _userService.AddCustomerAsync(createCustomerDTO);

            return Ok(user);
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet("GetAllCustomerUserAysnc")]
        public async Task<ActionResult<PaginatedResult<GetCustomerDTO>>> GetAllCustomerUserAysnc([FromQuery]CustomerQueryParams queryParams)
        {
            _logger.LogInformation("UpdateCustomerAddress request started");
            var Users = await _userService.GetAllCustomerUserAysnc(queryParams);
            return Ok(Users);
        }

        //[Authorize(Roles = SD.Role_Admin + " " + SD.Role_Customer)]
        [HttpPut("{id}/address")]
        public async Task<ActionResult<GetCustomerDTO>> UpdateCustomerAddress(string id, UpdateCustomerAddressDTO updateCustomerAddressDTO)
        {
            _logger.LogInformation("UpdateAddress request started");
            var result = await _userService.UpdateCustomerAddress(id, updateCustomerAddressDTO);
            return Ok(result);
        }


        //[Authorize(Roles = SD.Role_Admin + " " + SD.Role_Customer)]
        [HttpPut("{userId}/addresses")]
        public async Task<IActionResult> UpdateAddress(
            [FromRoute] string userId,
            [FromBody] UpdateAddressDto dto)
        {

            _logger.LogInformation("UpdateAddress request started");

            await _userService.UpdateAddressAsync(userId, dto);

                return Ok(new
                {
                    message = "Address updated successfully"
                });
            
           
        }

        //[Authorize(Roles = SD.Role_Admin + " " + SD.Role_Customer)]
        [HttpDelete("{userId}/addresses")]
        public async Task<IActionResult> DeleteAddress(
            [FromRoute] string userId,
            [FromBody] DeleteAddressDto dto)
        {

            _logger.LogInformation("DeleteAddress request started");

            await _userService.DeleteAddressAsync(userId, dto);

                return Ok(new
                {
                    message = "Address deleted successfully"
                });
            
           
        }

        //[Authorize(Roles = SD.Role_Admin + " " + SD.Role_Customer)]
        [HttpGet("my-addresses")]
        public async Task<IActionResult> GetMyAddresses([FromQuery] AddressQueryParams queryParams)
        {
            _logger.LogInformation("GetMyAddresses request started");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("GetMyAddresses failed: userId not found in claims");
                return Unauthorized();
            }
               

            queryParams.UserId = userId;

            var result = await _userService.GetUserAddressesAsync(queryParams);

            return Ok(result);
        }

    }    

}
