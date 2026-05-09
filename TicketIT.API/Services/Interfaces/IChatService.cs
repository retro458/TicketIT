using TicketIT.API.DTOs.Request.Chat;
using TicketIT.API.DTOs.Response.Chat;

namespace TicketIT.API.Services.Interfaces;

public interface IChatService
{
    Task<IEnumerable<MensajeChatResponseDto>> GetMensajesAsync(int ticketId);
    Task<MensajeChatResponseDto> EnviarMensajeAsync(EnviarMensajeRequestDto request, int emisorId);
    Task MarcarComoLeidoAsync(int ticketId, int usuarioId);
}