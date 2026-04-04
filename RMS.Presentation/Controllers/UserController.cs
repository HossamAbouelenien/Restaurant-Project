using Microsoft.AspNetCore.Mvc;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IUserServices;
using RMS.Shared.DTOs.UserDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController( IUserService userService)
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

            return CreatedAtAction("GetUserDetails", new { id = user.Id },user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDetailsDTO>> UpdateUserAsync(string id, UpdateUserDto updateUserDto)
        {
            if (id != updateUserDto.Id)
                return BadRequest("Id mismatch");

            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(user);
        }

    }
}
