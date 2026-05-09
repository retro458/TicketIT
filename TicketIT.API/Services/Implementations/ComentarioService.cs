using TicketIT.API.DTOs.Request.Comentario;
using TicketIT.API.DTOs.Response.Comentario;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;
using TicketIT.API.Services.Interfaces;

namespace TicketIT.API.Services.Implementations;

public class ComentarioService : IComentarioService
{
    private readonly IComentarioRepository _comentarioRepo;

    public ComentarioService(IComentarioRepository comentarioRepo)
    {
        _comentarioRepo = comentarioRepo;
    }

    public async Task<IEnumerable<ComentarioResponseDto>> GetByTicketIdAsync(int ticketId)
    {
        var comentarios = await _comentarioRepo.GetByTicketIdAsync(ticketId);
        return comentarios.Select(c => new ComentarioResponseDto
        {
            Id = c.Id,
            Contenido = c.Contenido,
            Usuario = c.Usuario?.Nombre ?? "",
            Tipo = c.Tipo?.Nombre ?? "",
            CreadoEn = c.CreadoEn ?? DateTime.UtcNow
        });
    }

    public async Task<ComentarioResponseDto> CrearAsync(CrearComentarioRequestDto request, int usuarioId)
    {
        var comentario = new Comentario
        {
            TicketId = request.TicketId,
            UsuarioId = usuarioId,
            TipoId = request.TipoId,
            Contenido = request.Contenido,
            CreadoEn = DateTime.UtcNow
        };

        var creado = await _comentarioRepo.CreateAsync(comentario);

        return new ComentarioResponseDto
        {
            Id = creado.Id,
            Contenido = creado.Contenido,
            Usuario = "",
            Tipo = "",
            CreadoEn = creado.CreadoEn ?? DateTime.UtcNow
        };
    }
}