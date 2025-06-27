using BE_MEGA_PROJECT.Models;
using BE_MEGA_PROJECT.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_MEGA_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {

        private readonly IPromotionService _service;

        public PromotionsController(IPromotionService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> Get() =>
        Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var promo = await _service.GetById(id);
            if (promo == null) return NotFound();
            return Ok(promo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Promotion promo)
        {
            var created = await _service.Create(promo);
            return Ok(created);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Promotion promo)
        {
            var updated = await _service.Update(promo);
            return updated ? Ok(promo) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.Delete(id);
            return deleted ? Ok() : NotFound();
        }
    }
}
