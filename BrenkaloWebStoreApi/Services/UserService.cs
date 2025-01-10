using BrenkaloWebStoreApi.Data;
using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BrenkaloWebStoreApi.Services
{
    public class UserService : IUserService
    {
        private readonly WebStoreContext _context;

        public UserService(WebStoreContext context)
        {
            _context = context;
        }

        // Get User by Username
        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return null; 
            }

            var userDto = new UserDto
            {
                Username = user.Username,
                UserRole = user.UserRole,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                UserAddresses = user.UserAddresses.Select(ua => new UserAddressDto
                {
                    AddressLine1 = ua.AddressLine1,
                    AddressLine2 = ua.AddressLine2,
                    City = ua.City,
                    State = ua.State,
                    PostalCode = ua.PostalCode,
                    Country = ua.Country,
                    IsDefault = ua.IsDefault
                }).ToList()
            };

            return userDto;
        }

        // Get user by email
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // Update User
        public async Task<bool> UpdateUserAsync(string username, UserDto userDto)
        {
            var user = await _context.Users
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return false; 
            }

            // Update user details
            user.UserRole = userDto.UserRole ?? user.UserRole;
            user.Firstname = userDto.Firstname ?? user.Firstname;
            user.Lastname = userDto.Lastname ?? user.Lastname;
            user.Email = userDto.Email ?? user.Email;

            // Update user addresses if needed
            if (userDto.UserAddresses != null)
            {
                foreach (var addressDto in userDto.UserAddresses)
                {
                    var existingAddress = user.UserAddresses.FirstOrDefault(ua => ua.UserId == user.Id);

                    if (existingAddress != null)
                    {
                        // Update existing address
                        existingAddress.AddressLine1 = addressDto.AddressLine1 ?? existingAddress.AddressLine1;
                        existingAddress.AddressLine2 = addressDto.AddressLine2 ?? existingAddress.AddressLine2;
                        existingAddress.City = addressDto.City ?? existingAddress.City;
                        existingAddress.State = addressDto.State ?? existingAddress.State;
                        existingAddress.PostalCode = addressDto.PostalCode ?? existingAddress.PostalCode;
                        existingAddress.Country = addressDto.Country ?? existingAddress.Country;
                        existingAddress.IsDefault = addressDto.IsDefault ?? existingAddress.IsDefault;
                    }
                    else
                    {
                        // Add new address
                        var newAddress = new UserAddress
                        {
                            AddressLine1 = addressDto.AddressLine1,
                            AddressLine2 = addressDto.AddressLine2,
                            City = addressDto.City,
                            State = addressDto.State,
                            PostalCode = addressDto.PostalCode,
                            Country = addressDto.Country,
                            IsDefault = addressDto.IsDefault,
                            UserId = user.Id
                        };
                        user.UserAddresses.Add(newAddress);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return true; 
        }
    }
}
