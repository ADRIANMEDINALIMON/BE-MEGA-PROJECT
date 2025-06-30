using BE_MEGA_PROJECT.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using BE_MEGA_PROJECT.Models;

namespace BE_MEGA_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscribersController : ControllerBase
    {
        private readonly ISubscriberRepository _repo;

        public SubscribersController(ISubscriberRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscriber>>> Get()
        {
            var list = await _repo.GetAllSubscribers();
            return Ok(list);
        }
    }
}
