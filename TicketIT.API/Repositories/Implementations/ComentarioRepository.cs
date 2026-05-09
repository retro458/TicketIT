using Microsoft.EntityFrameworkCore;
using TicketIT.API.Data;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;

namespace TicketIT.API.Repositories.Implementations;

public class ComentarioRepository : GenericRepository<Comentario>, IComentarioRepository
{
    public ComentarioRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Comentario>> GetByTicketIdAsync(int ticketId) =>
        await _dbSet.Where(c => c.TicketId == ticketId)
                    .Include(c => c.Usuario)
                    .Include(c => c.Tipo)
                    .OrderBy(c => c.CreadoEn)
                    .ToListAsync();
}