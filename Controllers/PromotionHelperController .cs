using BE_MEGA_PROJECT.Data;
using BE_MEGA_PROJECT.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BE_MEGA_PROJECT.DTOs.PromotionHelperDTOs;

namespace BE_MEGA_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionHelperController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PromotionHelperController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("cities")]
        public async Task<ActionResult<List<DropdownOptionDTO>>> GetCities()
        {
            var cities = await _context.Cities
                .Select(c => new DropdownOptionDTO
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Ok(cities);
        }

        [HttpGet("neighborhoods")]
        public async Task<ActionResult<List<NeighborhoodDropdownDTO>>> GetNeighborhoods()
        {
            var neighborhoods = await _context.Neighborhoods
                .Include(n => n.City)
                .Select(n => new NeighborhoodDropdownDTO
                {
                    Id = n.Id,
                    Name = n.Name,
                    CityId = n.CityId,
                    CityName = n.City.Name,
                    DisplayName = $"{n.Name} ({n.City.Name})"
                })
                .OrderBy(n => n.CityName)
                .ThenBy(n => n.Name)
                .ToListAsync();

            return Ok(neighborhoods);
        }

       

        [HttpGet("branches")]
        public async Task<ActionResult<List<BranchDropdownDTO>>> GetBranches()
        {
            var branches = await _context.Branches
                .Select(b => new BranchDropdownDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Address = b.Address,
                    DisplayName = $"{b.Name} - {b.Address}"
                })
                .OrderBy(b => b.Name)
                .ToListAsync();

            return Ok(branches);
        }

        [HttpGet("packages")]
        public async Task<ActionResult<List<PackageDropdownDTO>>> GetPackages()
        {
            var packages = await _context.Packages
                .Include(p => p.PackageServices)
                    .ThenInclude(ps => ps.Service)
                .Select(p => new PackageDropdownDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Services = p.PackageServices.Select(ps => ps.Service.Name).ToList(),
                    ServicesDisplay = string.Join(", ", p.PackageServices.Select(ps => ps.Service.Name))
                })
                .OrderBy(p => p.Name)
                .ToListAsync();

            return Ok(packages);
        }

        [HttpGet("services")]
        public async Task<ActionResult<List<ServiceDropdownDTO>>> GetServices()
        {
            var services = await _context.Services
                .Select(s => new ServiceDropdownDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    MonthlyPrice = s.MonthlyPrice,
                    SetupPrice = s.SetupPrice,
                    DisplayName = $"{s.Name} (${s.MonthlyPrice}/mes)"
                })
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Ok(services);
        }

        [HttpGet("target-types")]
        public ActionResult<List<TargetTypeOptionDTO>> GetTargetTypes()
        {
            var targetTypes = new List<TargetTypeOptionDTO>
            {
                new TargetTypeOptionDTO { Value = "CITY", Label = "Ciudad", Description = "Aplica a todos los suscriptores de una ciudad" },
                new TargetTypeOptionDTO { Value = "NEIGHBORHOOD", Label = "Colonia", Description = "Aplica a todos los suscriptores de una colonia" },
                new TargetTypeOptionDTO { Value = "BRANCH", Label = "Sucursal", Description = "Aplica a todos los suscriptores de una sucursal" },
                new TargetTypeOptionDTO { Value = "PACKAGE", Label = "Paquete", Description = "Aplica a todos los suscriptores con un paquete específico" }
            };

            return Ok(targetTypes);
        }

        [HttpGet("discount-types")]
        public ActionResult<List<DiscountTypeOptionDTO>> GetDiscountTypes()
        {
            var discountTypes = new List<DiscountTypeOptionDTO>
            {
                new DiscountTypeOptionDTO { Value = "PERCENT", Label = "Porcentaje (%)", Description = "Descuento en porcentaje del monto" },
                new DiscountTypeOptionDTO { Value = "FIXED", Label = "Monto Fijo ($)", Description = "Descuento de cantidad fija en pesos" }
            };

            return Ok(discountTypes);
        }

      
    
}
}
