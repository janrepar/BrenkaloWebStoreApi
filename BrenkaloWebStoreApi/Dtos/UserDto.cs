﻿namespace BrenkaloWebStoreApi.Dtos
{
    public class UserDto
    {
        public string Username { get; set; } = null!;

        public string UserRole { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string Email { get; set; } = null!;

        public string Tel {  get; set; } = null!;

        public List<UserAddressDto>? UserAddresses { get; set; } = new List<UserAddressDto>();
    }
}
