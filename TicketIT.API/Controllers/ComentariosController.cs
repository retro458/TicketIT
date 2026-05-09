using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketIT.API.DTOs.Request.Comentario;
using TicketIT.API.Services.Interfaces;

namespace TicketIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ComentariosController : ControllerBase
{
    private readonly IComentarioService _comentarioService;

    public ComentariosController(IComentarioService comentarioService)
    {
        _comentarioService = comentarioService;
    }

    [HttpGet("ticket/{ticketId}")]
    public async Task<IActionResult> GetByTicket(int ticketId) =>
        Ok(await _comentarioService.GetByTicketIdAsync(ticketId));

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearComentarioRequestDto request)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var comentario = await _comentarioService.CrearAsync(request, usuarioId);
        return CreatedAtAction(nameof(GetByTicket), new { ticketId = request.TicketId }, comentario);
    }
}