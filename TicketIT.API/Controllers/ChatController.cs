using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketIT.API.DTOs.Request.Chat;
using TicketIT.API.Services.Interfaces;

namespace TicketIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("{ticketId}")]
    public async Task<IActionResult> GetMensajes(int ticketId) =>
        Ok(await _chatService.GetMensajesAsync(ticketId));

    [HttpPost]
    public async Task<IActionResult> EnviarMensaje([FromBody] EnviarMensajeRequestDto request)
    {
        var emisorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var mensaje = await _chatService.EnviarMensajeAsync(request, emisorId);
        return CreatedAtAction(nameof(GetMensajes), new { ticketId = request.TicketId }, mensaje);
    }

    [HttpPatch("{ticketId}/leido")]
    public async Task<IActionResult> MarcarLeido(int ticketId)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _chatService.MarcarComoLeidoAsync(ticketId, usuarioId);
        return NoContent();
    }
}