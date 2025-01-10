using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;

namespace BrenkaloWebStoreApi.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserAsync(string username, UserDto userDto);
        Task<User> GetUserByEmailAsync(string email);
    }
}
