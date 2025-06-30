// Services/Interfaces/ISubscriberService.cs
using BE_MEGA_PROJECT.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE_MEGA_PROJECT.Services.Interfaces
{
    public interface ISubscriberService
    {
        Task<IEnumerable<Subscriber>> GetAll();
    }
}
