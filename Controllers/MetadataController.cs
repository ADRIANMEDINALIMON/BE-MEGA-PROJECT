// Controllers/MetadataController.cs
using Microsoft.AspNetCore.Mvc;
using BE_MEGA_PROJECT.Data;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class MetadataController : ControllerBase
{
    private readonly ApplicationDbContext _ctx;
    public MetadataController(ApplicationDbContext ctx) => _ctx = ctx;

    [HttpGet("cities")]
    public IActionResult GetCities() =>
        Ok(_ctx.Cities.Select(c => new { c.Id, c.Name }).ToList());

    [HttpGet("neighborhoods")]
    public IActionResult GetNeighborhoods() =>
        Ok(_ctx.Neighborhoods.Select(n => new { n.Id, n.Name, n.CityId }).ToList());

    [HttpGet("branches")]
    public IActionResult GetBranches() =>
        Ok(_ctx.Branches.Select(b => new { b.Id, b.Name }).ToList());

    [HttpGet("packages")]
    public IActionResult GetPackages()
    {
        // Usamos directamente el DbSet<Package> si existe
        var list = _ctx.Packages
            .Select(p => new { p.Id, p.Name })
            .ToList();
        return Ok(list);
    }
}
