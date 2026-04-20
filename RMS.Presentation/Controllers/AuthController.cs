using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction.IIdentityService;
using RMS.Shared.DTOs.IdentityDTOs;
using System.Security.Claims;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (registerationRequestDTO == null)
                {
                    // Test Log the error for debugging purposes
                    _logger.LogInformation("Registeration data is required");

                    return BadRequest("Registeration data is required");
                }

                if (await _authService.IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    return Conflict($"User with email '{registerationRequestDTO.Email}' already exists");
                }

                var user = await _authService.RegisterAsync(registerationRequestDTO);

                if (user == null)
                {
                    return BadRequest("Registration failed");
                }

                var response = new
                {
                    Message = "User registered successfully",
                    Data = user
                };

                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {

                    Message = "An error occurred during registration",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                if (loginRequestDTO == null)
                {
                    return BadRequest("Login data is required");
                }

                var loginResponse = await _authService.LoginAsync(loginRequestDTO);

                if (loginResponse == null)
                {
                    return BadRequest("Login failed. Please check your credentials.");
                }

                var response = new
                {
                    Message = "Login successfully",
                    Data = loginResponse
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred during login",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDTO>> RefreshAccessToken([FromBody] RefreshTokenRequestDTO refreshTokenRequestDTO)
        {
            try
            {
                if (refreshTokenRequestDTO == null || String.IsNullOrEmpty(refreshTokenRequestDTO.RefreshToken))
                {
                    return BadRequest("Refresh Token is required");
                }

                var tokenResponse = await _authService.RefreshAccessTokenAsync(refreshTokenRequestDTO);

                if (tokenResponse == null)
                {
                    return Unauthorized(new
                    {
                        StatusCode = 401,
                        Message = "Invalid or expired refresh token. If token reuse was detected, all your sessions have been terminated for security. Please login again."
                    });
                }

                var response = new
                {
                    Message = "Token refreshed successfully",
                    Data = tokenResponse
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred during token refresh",
                    Error = ex.Message
                });
            }
        }


        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
                return Unauthorized();
            var user = await _authService.GetCurrentUserAsync(email);
            return Ok(user);

        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UserDTO>> UpdateCurrentUserAsync(string email,[FromBody] UpdateCurrentUserDTO dto)
        {
            if (email == null)
                return Unauthorized();
            var updatedUser = await _authService.UpdateCurrentUserAsync(email, dto);
            return Ok(updatedUser);

        }

        // This endpoint is for demonstration purposes. In a real application, the confirmation link would be sent to the user's email.

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string? userId, string? code)
        {
            var result = await _authService.ConfirmEmailAsync(userId, code);

            if (result == "Success")
                return Ok("Email confirmed successfully");

            return BadRequest("Error confirming email");
        }

        // This endpoint is for demonstration purposes. In a real application, the reset code would be sent to the user's email. 
        [HttpPost("send-reset-code")]
        public async Task<IActionResult> SendResetCode(string email)
        {
            var result = await _authService.SendResetPasswordCode(email);
            return Ok(result);
        }

        // This endpoint is for demonstration purposes. In a real application, the verification would be done through a link in the user's email.
        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var result = await _authService.VerifyResetCode(code);

            if (result.result != "Valid")
                return BadRequest(result.result);

            return Ok(new { resetSessionToken = result.resetSessionToken });
        }

        // This endpoint is for demonstration purposes. In a real application, the reset session token would be validated and used to identify the user for password reset.
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            var result = await _authService.ResetPassword(
                dto.ResetSessionToken,
                dto.NewPassword,
                dto.ConfirmPassword);

            if (result != "Success")
                return BadRequest(result);

            return Ok(result);
        }



    }
}