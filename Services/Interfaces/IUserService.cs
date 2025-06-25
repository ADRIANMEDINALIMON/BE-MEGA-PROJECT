using BE_MEGA_PROJECT.Models;

namespace BE_MEGA_PROJECT.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> CreateUser(User user);

        Task<User?> Authenticate(string username, string password);
    }
}
