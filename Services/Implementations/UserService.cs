using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using BE_MEGA_PROJECT.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BE_MEGA_PROJECT.Services.Implementations
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) { 
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsers() =>
            await _userRepository.GetUsers();

        public async Task<User> CreateUser(User user) {
            return await _userRepository.CreateUser(user);
        }

        public async Task<User?> Authenticate(string username, string password) {
            return await _userRepository.Authenticate(username, password);
        }
    }
}
