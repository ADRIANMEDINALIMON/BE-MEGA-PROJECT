namespace BE_MEGA_PROJECT.DTOs
{
    public class CreateUserDTO
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string? FullName { get; set; }

    }
}
