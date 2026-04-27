using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction.IIdentityService;
using RMS.Shared.DTOs.IdentityDTOs;
using System.Net;
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
                    //Test Log the error for debugging purposes

                   //_logger.LogInformation("Registeration data is required");

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

                SetTokenCookies(loginResponse.AccessToken, loginResponse.RefreshToken, loginResponse.ExpiresAt);

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
        public async Task<ActionResult<UserDTO>> RefreshAccessToken()
        {
            try
            {
              
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                    return BadRequest("Refresh Token is required");

                var tokenResponse = await _authService.RefreshAccessTokenAsync(
                    new RefreshTokenRequestDTO { RefreshToken = refreshToken });

                if (tokenResponse == null)
                {
                   
                    ClearTokenCookies();
                    return Unauthorized(new
                    {
                        StatusCode = 401,
                        Message = "Invalid or expired refresh token. Please login again."
                    });
                }

                SetTokenCookies(tokenResponse.AccessToken, tokenResponse.RefreshToken, tokenResponse.ExpiresAt);

                return Ok(new
                {
                    Message = "Token refreshed successfully",
                    Data = tokenResponse
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred during token refresh", Error = ex.Message });
            }
        }


        //[HttpPost("logout")]
        //[Authorize]
        //public async Task<IActionResult> Logout()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];

        //    if (!string.IsNullOrEmpty(refreshToken))
        //        await _authService.RevokeRefreshTokenAsync(refreshToken);

        //    ClearTokenCookies();
        //    return Ok(new { Message = "Logged out successfully" });
        //}


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

        

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string? userId, string? code)
        {
            var result = await _authService.ConfirmEmailAsync(userId, code);

            if (result == "Success")
                return Ok("Email confirmed successfully");

            return BadRequest("Error confirming email");
        }

        
        [HttpPost("send-reset-code")]
        public async Task<IActionResult> SendResetCode([FromBody] string email)
        {
            var result = await _authService.SendResetPasswordCode(email);
            return Ok(result);
        }

        
        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var result = await _authService.VerifyResetCode(code);

            if (result.result != "Valid")
                return BadRequest(result.result);

            return Ok(new { resetSessionToken = result.resetSessionToken });
        }

       
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


        #region  External Login (Google / Facebook)

        [HttpGet("external-login")]
        public IActionResult ExternalLogin(string provider)
        {
            var normalizedProvider = provider?.Trim();

           
            var supportedProviders = new[] { "Google", "Facebook" };
            if (!supportedProviders.Contains(normalizedProvider, StringComparer.OrdinalIgnoreCase))
                return BadRequest("Unsupported provider");

            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), new { provider });

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };

            return Challenge(properties, provider);
        }


        [HttpGet("external-callback")]
        public async Task<IActionResult> ExternalLoginCallback([FromQuery] string provider)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(
                IdentityConstants.ExternalScheme
            );

            if (!authenticateResult.Succeeded)
                return BadRequest("OAuth failed");

            
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var result = await _authService.ExternalLoginAsync(
                authenticateResult.Principal,
                provider
            );

            if (result == null)
                return BadRequest("Login failed");

            SetTokenCookies(result.AccessToken, result.RefreshToken, result.ExpiresAt);

            return Redirect("http://localhost:4200/auth/auth-callback");
        }

        [HttpGet("external-cancelled")]
        public IActionResult ExternalCancelled()
        {
            return Ok(new { message = "Login was cancelled by the user." });
        }

        #endregion



        private void SetTokenCookies(string accessToken, string refreshToken, DateTime? expiresAt)
        {
            var accessTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(4) 
            };

            var refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("accessToken", accessToken, accessTokenOptions);
            Response.Cookies.Append("refreshToken", refreshToken, refreshTokenOptions);
        }

        private void ClearTokenCookies()
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");
        }
    }
}