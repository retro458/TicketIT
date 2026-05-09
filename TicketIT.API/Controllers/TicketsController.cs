using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketIT.API.DTOs.Request.Ticket;
using TicketIT.API.Services.Interfaces;

namespace TicketIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> GetAll() =>
        Ok(await _ticketService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ticket = await _ticketService.GetByIdAsync(id);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<IActionResult> GetByCliente(int clienteId) =>
        Ok(await _ticketService.GetByClienteIdAsync(clienteId));

    [HttpGet("tecnico/{tecnicoId}")]
    [Authorize(Roles = "Tecnico,Administrador")]
    public async Task<IActionResult> GetByTecnico(int tecnicoId) =>
        Ok(await _ticketService.GetByTecnicoIdAsync(tecnicoId));

    [HttpPost]
    [Authorize(Roles = "Cliente,Administrador")]
    public async Task<IActionResult> Crear([FromBody] CrearTicketRequestDto request)
    {
        var clienteId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var ticket = await _ticketService.CrearAsync(request, clienteId);
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Tecnico,Administrador")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarTicketRequestDto request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var ticket = await _ticketService.ActualizarAsync(id, request, userId);
        return ticket is null ? NotFound() : Ok(ticket);
    }
}