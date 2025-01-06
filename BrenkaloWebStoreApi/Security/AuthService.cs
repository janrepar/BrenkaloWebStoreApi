using BrenkaloWebStoreApi.Data;
using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BrenkaloWebStoreApi.Security
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly WebStoreContext _context;

        public AuthService(IConfiguration configuration, WebStoreContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // TODO Error handling

        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var userInDB = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (userInDB != null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Pwd))
            {
                return null; // Username already exists or invalid output
            }

            string passwordHash = CreatePasswordHash(request.Pwd);

            // Create the user object
            var user = new User
            {
                Username = request.Username,
                Pwd = passwordHash,
                UserRole = request.UserRole,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Enabled = 1
            };

            // Add user addresses if provided in the request
            if (request.UserAddresses != null && request.UserAddresses.Any())
            {
                foreach (var address in request.UserAddresses)
                {
                    user.UserAddresses.Add(new UserAddress
                    {
                        AddressLine1 = address.AddressLine1,
                        AddressLine2 = address.AddressLine2,
                        City = address.City,
                        State = address.State,
                        PostalCode = address.PostalCode,
                        Country = address.Country,
                        IsDefault = address.IsDefault,
                        Enabled = 1 
                    });
                }
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ActionResult<string>> Login(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !VerifyPasswordHash(request.Pwd, user.Pwd))
            {
                return null; // Invalid username or password
            }

            // Generate access and refresh tokens
            string accessToken = CreateToken(user);
            string refreshToken = CreateRefreshToken();

            // Save refresh token in the database
            var session = new UserSession
            {
                UserId = user.Id,
                Token = refreshToken,
                RequestIp = "UserIPAddress", 
                ValidUntil = DateTime.UtcNow.AddDays(7), 
            };

            await SaveSession(session);

            return new JsonResult(new { accessToken, refreshToken });
        }

        public async Task<ActionResult> ChangePassword(ChangePasswordDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found.");
            }

            if (!VerifyPasswordHash(request.OldPassword, user.Pwd))
            {
                return new UnauthorizedObjectResult("Old password is incorrect.");
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 5)
            {
                return new BadRequestObjectResult("New password is too weak. Minimum length is 6 characters.");
            }

            user.Pwd = CreatePasswordHash(request.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Password updated successfully.");
        }
        public async Task<ActionResult<string>> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return new UnauthorizedResult();
            }

            var session = await GetSessionByToken(refreshToken);
            if (session == null || session.ValidUntil < DateTime.UtcNow)
            {
                return new UnauthorizedResult(); // Token invalid or expired
            }

            // Generate new tokens
            string accessToken = CreateToken(session.User);
            string newRefreshToken = CreateRefreshToken();

            // Rotate refresh token
            session.Token = newRefreshToken;
            session.ValidUntil = DateTime.UtcNow.AddDays(7);
            session.CreatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return new JsonResult(new { accessToken, refreshToken = newRefreshToken });
        }

        public async Task<UserSession?> GetSessionByToken(string refreshToken)
        {
            return await _context.UserSessions
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Token == refreshToken && s.ValidUntil > DateTime.UtcNow);
        }

        public async Task SaveSession(UserSession session)
        {
            _context.UserSessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public async Task InvalidateSession(int sessionId)
        {
            var session = await _context.UserSessions.FindAsync(sessionId);
            if (session != null)
            {
                _context.UserSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
        }

        private string CreateRefreshToken()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserRole) 
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:Key").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        // Create password hash and return it as a Base64 string
        private string CreatePasswordHash(string password)
        {
            using (var sha512 = SHA512.Create())
            {
                var hash = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash); // Encode as Base64 string
            }
        }

        // Verify password hash by decoding the Base64 string
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha512 = SHA512.Create())
            {
                var computedHash = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var storedHashBytes = Convert.FromBase64String(storedHash); // Decode Base64 string
                return computedHash.SequenceEqual(storedHashBytes);
            }
        }
    }
}
