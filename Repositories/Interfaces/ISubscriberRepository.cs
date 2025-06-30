// Repositories/Interfaces/ISubscriberRepository.cs
using BE_MEGA_PROJECT.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_MEGA_PROJECT.Repositories.Interfaces
{
    public interface ISubscriberRepository
    {
        Task<IEnumerable<Subscriber>> GetAllSubscribers();
        // aquí podrías añadir FindById, Create, etc.
    }
}
