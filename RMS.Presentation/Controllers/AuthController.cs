using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.Services.Exceptions;
using RMS.ServicesAbstraction.IServices.IIdentityService;
using RMS.Shared.DTOs.IdentityDTOs;
using System.Security.Claims;

[ApiController]
[Route("api/[Controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterationRequestDTO registerationRequestDTO)
    {

        _logger.LogInformation("Register request started");

        if (registerationRequestDTO == null)
            return BadRequest("Registeration data is required");

        var user = await _authService.RegisterAsync(registerationRequestDTO);

        _logger.LogInformation("User registered successfully: {Email}", user.Email);

        var response = new
        {
            Message = "User registered successfully",
            Data = user
        };

        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDTO>> Login([FromBody] LoginRequestDTO loginRequestDTO)
    {
        _logger.LogInformation("Login attempt started");

        var loginResponse = await _authService.LoginAsync(loginRequestDTO);

        _logger.LogInformation("Login success for user");

        if (loginResponse == null)
        {
            _logger.LogWarning("Login failed: invalid credentials");
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

    [HttpPost("refresh-token")]
    public async Task<ActionResult<UserDTO>> RefreshAccessToken()
    {

        _logger.LogInformation("Refresh token request started");

        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            _logger.LogWarning("Refresh token is missing");
            return BadRequest("Refresh Token is required");

        }
            

        var tokenResponse = await _authService.RefreshAccessTokenAsync(
            new RefreshTokenRequestDTO { RefreshToken = refreshToken });

        _logger.LogInformation("Refresh token succeeded");

        if (tokenResponse == null)
        {
            _logger.LogWarning("Refresh token invalid or expired");

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

    //[Authorize]
    [HttpGet("CurrentUser")]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
        _logger.LogInformation("GetCurrentUser request started");

        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (email == null)
        {
            _logger.LogWarning("Unauthorized access: email claim is missing");
            throw new UnauthorizedDriverException();

        }
            

        var user = await _authService.GetCurrentUserAsync(email);
        _logger.LogInformation("Current user fetched successfully");

        return Ok(user);
    }

    //[Authorize]
    [HttpPut]
    public async Task<ActionResult<UserDTO>> UpdateCurrentUserAsync(string email, [FromBody] UpdateCurrentUserDTO dto)
    {
        _logger.LogInformation("UpdateCurrentUser request started");

        if (email == null)
        {
            _logger.LogWarning("UpdateCurrentUser failed: email is null (unauthorized)");
            throw new UnauthorizedDriverException();
        }
            

        var updatedUser = await _authService.UpdateCurrentUserAsync(email, dto);
        _logger.LogInformation("User updated successfully: {Email}", email);
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
        _logger.LogInformation("Send reset password code started for email: {Email}", email);

        var result = await _authService.SendResetPasswordCode(email);

        _logger.LogInformation("Reset code sent successfully to email: {Email}", email);
        return Ok(result);
    }

    [HttpPost("verify-reset-code")]
    public async Task<IActionResult> VerifyCode(string code)
    {
        _logger.LogInformation("Verify reset code started");
        var result = await _authService.VerifyResetCode(code);

        if (result.result != "Valid")
        {
            _logger.LogWarning("Invalid reset code provided");
            return BadRequest(result.result);

        }

        _logger.LogInformation("Reset code verified successfully");
        return Ok(new { resetSessionToken = result.resetSessionToken });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
    {
        _logger.LogInformation("Reset password process started");
        var result = await _authService.ResetPassword(
            dto.ResetSessionToken,
            dto.NewPassword,
            dto.ConfirmPassword);

        if (result != "Success")
        {
            _logger.LogWarning("Reset password failed");
            return BadRequest(result);

        }

        _logger.LogInformation("Password reset successfully");
        return Ok(result);
    }

    #region External Login (Google / Facebook)

    [HttpGet("external-login")]
    public IActionResult ExternalLogin(string provider)
    {
        _logger.LogInformation("External login started with provider: {Provider}", provider);

        var normalizedProvider = provider?.Trim();

        var supportedProviders = new[] { "Google", "Facebook" };
        if (!supportedProviders.Contains(normalizedProvider, StringComparer.OrdinalIgnoreCase))
        {
            _logger.LogWarning("External login failed: unsupported provider {Provider}", provider);
            return BadRequest("Unsupported provider");

        }
            

        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), new { provider });

        var properties = new AuthenticationProperties
        {
            RedirectUri = redirectUrl
        };

        _logger.LogInformation("Redirecting to external provider: {Provider}", provider);

        return Challenge(properties, provider);
    }

    [HttpGet("external-callback")]
    public async Task<IActionResult> ExternalLoginCallback([FromQuery] string provider)
    {
        _logger.LogInformation("External callback started for provider: {Provider}", provider);

        var authenticateResult = await HttpContext.AuthenticateAsync(
            IdentityConstants.ExternalScheme);

        if (!authenticateResult.Succeeded)
        {
            _logger.LogWarning("External login failed during authentication for provider: {Provider}", provider);
            return BadRequest("OAuth failed");

        }
            

        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        var result = await _authService.ExternalLoginAsync(
            authenticateResult.Principal,
            provider);

        if (result == null)
        {
            _logger.LogWarning("External login service returned null for provider: {Provider}", provider);
            return BadRequest("Login failed");

        }
            

        SetTokenCookies(result.AccessToken, result.RefreshToken, result.ExpiresAt);

        _logger.LogInformation("External login successful for provider: {Provider}", provider);

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