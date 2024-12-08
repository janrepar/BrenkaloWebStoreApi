using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using BrenkaloWebStoreApi.Security;
using Microsoft.AspNetCore.Http;
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

            if (user.Value == null)
            {
                return BadRequest("User already exists or invalid input provided.");
            }

            return Ok(user.Value);
        }

        // Login an existing user
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(UserDto request)
        {
            var loginResult = await _authService.Login(request);

            if (loginResult.Result == null)
            {
                return BadRequest("Invalid username or password.");
            }

            return loginResult.Value; // Includes access token and refresh token
        }

        // Refresh the access token using a valid refresh token
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
