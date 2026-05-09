using TicketIT.API.DTOs.Request.Chat;
using TicketIT.API.DTOs.Response.Chat;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;
using TicketIT.API.Services.Interfaces;

namespace TicketIT.API.Services.Implementations;

public class ChatService : IChatService
{
    private readonly IMensajeChatRepository _chatRepo;

    public ChatService(IMensajeChatRepository chatRepo)
    {
        _chatRepo = chatRepo;
    }

    public async Task<IEnumerable<MensajeChatResponseDto>> GetMensajesAsync(int ticketId)
    {
        var mensajes = await _chatRepo.GetByTicketIdAsync(ticketId);
        return mensajes.Select(MapToDto);
    }

    public async Task<MensajeChatResponseDto> EnviarMensajeAsync(EnviarMensajeRequestDto request, int emisorId)
    {
        var mensaje = new MensajesChat
        {
            TicketId = request.TicketId,
            EmisorId = emisorId,
            Contenido = request.Contenido,
            TipoMensaje = request.TipoMensaje,
            EsPrivado = request.EsPrivado,
            Leido = false,
            CreadoEn = DateTime.UtcNow
        };

        var creado = await _chatRepo.CreateAsync(mensaje);
        return MapToDto(creado);
    }

    public async Task MarcarComoLeidoAsync(int ticketId, int usuarioId) =>
        await _chatRepo.MarkAsReadAsync(ticketId, usuarioId);

    private static MensajeChatResponseDto MapToDto(MensajesChat m) => new()
    {
        Id = m.Id,
        Contenido = m.Contenido,
        Emisor = m.Emisor?.Nombre ?? "",
        EmisorId = m.EmisorId,
        TipoMensaje = m.TipoMensaje ?? "text",
        Leido = m.Leido ?? false,
        EsPrivado = m.EsPrivado ?? false,
        CreadoEn = m.CreadoEn ?? DateTime.UtcNow
    };
}