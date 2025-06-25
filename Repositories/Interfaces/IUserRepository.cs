using BE_MEGA_PROJECT.Models;

namespace BE_MEGA_PROJECT.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> CreateUser(User user);

        Task<User?> Authenticate(string username, string password);


    }
}
