using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrenkaloWebStoreApi.Security
{
    public interface IAuthService
    {
        Task<ActionResult<User>> Register(UserDto request);
        Task<ActionResult<string>> Login(UserDto request);
        Task<ActionResult<string>> RefreshToken(string refreshToken);
        Task<UserSession?> GetSessionByToken(string refreshToken);
        Task SaveSession(UserSession session);
        Task InvalidateSession(int sessionId);
    }
}
