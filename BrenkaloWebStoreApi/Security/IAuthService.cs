using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrenkaloWebStoreApi.Security
{
    public interface IAuthService
    {
        Task<ActionResult> Register(RegisterDto request);
        Task<ActionResult<string>> Login(LoginDto request);
        Task<ActionResult> ChangePassword(ChangePasswordDto request);
        Task<ActionResult> ResetPassword(string email);
        Task<ActionResult<string>> RefreshToken(string refreshToken);
        Task<UserSession?> GetSessionByToken(string refreshToken);
        Task SaveSession(UserSession session);
        Task InvalidateSession(int sessionId);
    }
}
