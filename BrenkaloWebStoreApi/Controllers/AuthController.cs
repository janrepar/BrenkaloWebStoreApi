using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using BrenkaloWebStoreApi.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BrenkaloWebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // TODO Error handling

        // Register a new user
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await _authService.Register(request);

            if (user == null)
            {
                return BadRequest("User already exists or invalid input provided.");
            }

            return Ok(user.Value);
        }

        // Login an existing user
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginDto request)
        {
            var loginResult = await _authService.Login(request);

            if (loginResult == null)
            {
                return BadRequest("Invalid username or password.");
            }

            return loginResult.Result; // Includes access token and refresh token
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto request)
        {
            var result = await _authService.ChangePassword(request);

            if (result is BadRequestObjectResult || result is UnauthorizedObjectResult)
            {
                return result; // Return the error from the service
            }

            return Ok("Password updated successfully.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Dtos.ResetPasswordDto request)
        {
            var result = await _authService.ResetPassword(request.Email);

            // If the email doesn't exist or any other error occurs, it will be handled in AuthService
            return result;
        }

        // Refresh the access token using a valid refresh token
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<object>> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            var result = await _authService.RefreshToken(refreshToken);

            if (result.Result is UnauthorizedResult)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            return result.Value; // Includes new access token and refresh token
        }

        // Logout user and invalidate the refresh token
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is required.");
            }

            var session = await _authService.GetSessionByToken(refreshToken);
            if (session == null)
            {
                return NotFound("Session not found.");
            }

            await _authService.InvalidateSession(session.Id);

            return Ok("Logged out successfully.");
        }
    }
}
