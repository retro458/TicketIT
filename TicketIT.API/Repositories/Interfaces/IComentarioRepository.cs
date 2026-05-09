using TicketIT.API.Models;

namespace TicketIT.API.Repositories.Interfaces;

public interface IComentarioRepository : IGenericRepository<Comentario>
{
    Task<IEnumerable<Comentario>> GetByTicketIdAsync(int ticketId);
}