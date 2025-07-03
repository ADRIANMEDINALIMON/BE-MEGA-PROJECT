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
        public async Task<IActionResult> Get() {
            var entities = await _service.GetAll();

            var dtoList = entities.Select(p => new UpdatePromotionDTO
            {
                Name = p.Name,
                Description = p.Description,
                DiscountType = p.DiscountType,
                DiscountAmount = p.DiscountAmount,
                TargetType = p.TargetType,
                TargetId = p.TargetId,
                AppliesTo = p.AppliesTo,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Active = p.Active,
                ServiceId = p.ServiceId,
                Id = p.Id
            }).ToList();

            return Ok(dtoList);

        }

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
        public async Task<IActionResult> Put([FromBody] UpdatePromotionDTO dto)
        {
            var promotion = new Promotion
            {
                Id = dto.Id,
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

            try {
                var updated = await _service.Update(promotion);
                return updated ? Ok(promotion) : BadRequest();
            }
            catch (ArgumentException ex) {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor.", details = ex.Message });

            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                var deleted = await _service.Delete(id);
                if (!deleted)
                    return NotFound(new { message = "Promoción no encontrada." });

                return Ok(new { message = "Promoción eliminada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor.", details = ex.Message });
            }

        }
    }
}
