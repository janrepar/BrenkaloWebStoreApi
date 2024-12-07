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

        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var userInDB = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (userInDB != null)
            {
                return null; // Username already exists
            }

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Pwd))
            {
                return null; // Invalid input
            }

            string passwordHash = CreatePasswordHash(request.Pwd);

            var user = new User
            {
                Username = request.Username,
                Pwd = passwordHash,
                UserRole = request.UserRole, // Assign role to the user
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !VerifyPasswordHash(request.Pwd, user.Pwd))
            {
                return null; // Username or password is incorrect
            }

            string token = CreateToken(user);
            return token;
        }

        public async Task<ActionResult<string>> RefreshToken()
        {
            var user = new User();
            string token = CreateToken(user);
            return token;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserRole) 
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        // Create password hash and return it as a Base64 string
        private string CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash); // Encode as Base64 string
            }
        }

        // Verify password hash by decoding the Base64 string
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var storedHashBytes = Convert.FromBase64String(storedHash); // Decode Base64 string
                return computedHash.SequenceEqual(storedHashBytes);
            }
        }
    }
}
