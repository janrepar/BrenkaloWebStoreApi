namespace BrenkaloWebStoreApi.Dtos
{
    public class UserDto
    {
        public string Username { get; set; } = null!;

        public string UserRole { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string Pwd { get; set; } = null!;

        public string Email { get; set; } = null!;

    }
}
