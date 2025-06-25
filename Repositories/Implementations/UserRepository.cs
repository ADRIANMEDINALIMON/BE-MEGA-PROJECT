using BE_MEGA_PROJECT.Data;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BE_MEGA_PROJECT.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers() =>
            await _context.Users.ToListAsync();

        public async Task<User> CreateUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<User?> Authenticate(string username, string password) {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash==password);
            if (user == null) {
                return null;
            } 
            return user;
        }
    }
}