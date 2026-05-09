using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TicketIT.API.DTOs.Request.Chat;
using TicketIT.API.Services.Interfaces;

namespace TicketIT.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;

    public ChatHub(IChatService chatService)
    {
        _chatService = chatService;
    }

    // El cliente se une al grupo del ticket
    public async Task UnirseATicket(int ticketId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
        await Clients.Caller.SendAsync("Conectado", $"Conectado al ticket {ticketId}");
    }

    // El cliente sale del grupo del ticket
    public async Task SalirDeTicket(int ticketId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"ticket-{ticketId}");
    }

    // Enviar mensaje al grupo del ticket
    public async Task EnviarMensaje(EnviarMensajeRequestDto request)
    {
        var emisorId = int.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var emisorNombre = Context.User!.FindFirstValue(ClaimTypes.Name)!;

        var mensaje = await _chatService.EnviarMensajeAsync(request, emisorId);

        // Broadcast a todos los conectados al ticket
        await Clients.Group($"ticket-{request.TicketId}")
                     .SendAsync("RecibirMensaje", mensaje);
    }

    // Notificar que el usuario está escribiendo
    public async Task Escribiendo(int ticketId)
    {
        var nombre = Context.User!.FindFirstValue(ClaimTypes.Name)!;
        await Clients.OthersInGroup($"ticket-{ticketId}")
                     .SendAsync("UsuarioEscribiendo", nombre);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}