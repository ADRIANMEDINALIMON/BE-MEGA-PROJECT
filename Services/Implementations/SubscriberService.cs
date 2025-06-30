using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Repositories.Interfaces;
using BE_MEGA_PROJECT.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_MEGA_PROJECT.Services.Implementations
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepository _repo;

        public SubscriberService(ISubscriberRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Subscriber>> GetAll() =>
            _repo.GetAllSubscribers();
    }
}