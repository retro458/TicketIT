using Microsoft.EntityFrameworkCore;
using TicketIT.API.Data;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;

namespace TicketIT.API.Repositories.Implementations;

public class MensajeChatRepository : GenericRepository<MensajesChat>, IMensajeChatRepository
{
    public MensajeChatRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<MensajesChat>> GetByTicketIdAsync(int ticketId) =>
        await _dbSet.Where(m => m.TicketId == ticketId)
                    .Include(m => m.Emisor)
                    .OrderBy(m => m.CreadoEn)
                    .ToListAsync();

    public async Task<int> GetUnreadCountAsync(int ticketId, int usuarioId) =>
        await _dbSet.CountAsync(m => m.TicketId == ticketId && m.EmisorId != usuarioId && m.Leido == false);

    public async Task MarkAsReadAsync(int ticketId, int usuarioId)
    {
        var mensajes = await _dbSet
            .Where(m => m.TicketId == ticketId && m.EmisorId != usuarioId && m.Leido == false)
            .ToListAsync();

        mensajes.ForEach(m => m.Leido = true);
        await _context.SaveChangesAsync();
    }
}