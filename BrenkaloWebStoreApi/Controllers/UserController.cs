using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using BrenkaloWebStoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrenkaloWebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Get user by username
        [HttpGet("{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);          
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Update user
        [HttpPut("{username}")]
        public async Task<ActionResult> UpdateUser(string username, UserDto userDto)
        {
            var result = await _userService.UpdateUserAsync(username, userDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
