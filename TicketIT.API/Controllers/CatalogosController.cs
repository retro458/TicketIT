using Microsoft.AspNetCore.Mvc;
using TicketIT.API.Data;
using Microsoft.EntityFrameworkCore;

namespace TicketIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatalogosController : ControllerBase
{
    private readonly AppDbContext _context;

    public CatalogosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("prioridades")]
    public async Task<IActionResult> GetPrioridades() =>
        Ok(await _context.Prioridades.Select(p => new { p.Id, p.Nombre }).ToListAsync());

    [HttpGet("categorias")]
    public async Task<IActionResult> GetCategorias() =>
        Ok(await _context.Categorias.Select(c => new { c.Id, c.Nombre }).ToListAsync());

    [HttpGet("estados")]
    public async Task<IActionResult> GetEstados() =>
        Ok(await _context.Estados.Select(e => new { e.Id, e.Nombre }).ToListAsync());

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles() =>
        Ok(await _context.Roles.Select(r => new { r.Id, r.Nombre }).ToListAsync());
}