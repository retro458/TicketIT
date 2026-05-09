using TicketIT.API.Models;

namespace TicketIT.API.Repositories.Interfaces;

public interface IMensajeChatRepository : IGenericRepository<MensajesChat>
{
    Task<IEnumerable<MensajesChat>> GetByTicketIdAsync(int ticketId);
    Task<int> GetUnreadCountAsync(int ticketId, int usuarioId);
    Task MarkAsReadAsync(int ticketId, int usuarioId);
}