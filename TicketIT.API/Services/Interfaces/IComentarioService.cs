using TicketIT.API.DTOs.Request.Comentario;
using TicketIT.API.DTOs.Response.Comentario;

namespace TicketIT.API.Services.Interfaces;

public interface IComentarioService
{
    Task<IEnumerable<ComentarioResponseDto>> GetByTicketIdAsync(int ticketId);
    Task<ComentarioResponseDto> CrearAsync(CrearComentarioRequestDto request, int usuarioId);
}
