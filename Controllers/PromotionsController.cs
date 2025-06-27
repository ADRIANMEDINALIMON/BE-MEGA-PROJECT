using BE_MEGA_PROJECT.DTOs;
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
        public async Task<IActionResult> Post([FromBody] CreatePromotionDTO dto)
        {
            var promotion = new Promotion
            {
                Name = dto.Name,
                Description = dto.Description,
                DiscountType = dto.DiscountType,
                DiscountAmount = dto.DiscountAmount,
                TargetType = dto.TargetType,
                TargetId = dto.TargetId,
                AppliesTo = dto.AppliesTo,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Active = dto.Active,
                ServiceId = dto.ServiceId
            };
            try
            {
                var created = await _service.Create(promotion);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });

            }
            catch (Exception ex) {
                // Para errores inesperados (BD caída, etc.)
                return StatusCode(500, new { message = "Error interno del servidor.", details = ex.Message });

            }
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
