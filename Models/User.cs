namespace BE_MEGA_PROJECT.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string? FullName { get; set; }

        public DateTime CreatedAt { get; set; }

        public Boolean Active { get; set; } = true;

    }
}
