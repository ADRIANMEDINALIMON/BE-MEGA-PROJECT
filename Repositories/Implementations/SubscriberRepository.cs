// Repositories/Implementations/SubscriberRepository.cs
using BE_MEGA_PROJECT.Data;
using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_MEGA_PROJECT.Repositories.Implementations
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly ApplicationDbContext _context;
        public SubscriberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subscriber>> GetAllSubscribers()
        {
            return await _context.Subscribers
                                 .Include(s => s.City)
                                 .Include(s => s.Neighborhood)
                                 .Include(s => s.Branch)
                                 .ToListAsync();
        }
    }
}
