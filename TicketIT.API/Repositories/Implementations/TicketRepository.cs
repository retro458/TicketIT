using Microsoft.EntityFrameworkCore;
using TicketIT.API.Data;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;

namespace TicketIT.API.Repositories.Implementations;

public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Ticket>> GetByClienteIdAsync(int clienteId) =>
        await _dbSet.Where(t => t.ClienteId == clienteId)
                    .Include(t => t.Estado)
                    .Include(t => t.Prioridad)
                    .Include(t => t.Categoria)
                    .OrderByDescending(t => t.CreadoEn)
                    .ToListAsync();

    public async Task<IEnumerable<Ticket>> GetByTecnicoIdAsync(int tecnicoId) =>
        await _dbSet.Where(t => t.TecnicoId == tecnicoId)
                    .Include(t => t.Estado)
                    .Include(t => t.Prioridad)
                    .Include(t => t.Categoria)
                    .Include(t => t.Cliente)
                    .OrderByDescending(t => t.CreadoEn)
                    .ToListAsync();

    public async Task<IEnumerable<Ticket>> GetByEstadoIdAsync(int estadoId) =>
        await _dbSet.Where(t => t.EstadoId == estadoId)
                    .Include(t => t.Cliente)
                    .Include(t => t.Tecnico)
                    .Include(t => t.Prioridad)
                    .OrderByDescending(t => t.CreadoEn)
                    .ToListAsync();

    public async Task<IEnumerable<Ticket>> GetByPrioridadIdAsync(int prioridadId) =>
        await _dbSet.Where(t => t.PrioridadId == prioridadId)
                    .Include(t => t.Estado)
                    .Include(t => t.Cliente)
                    .Include(t => t.Tecnico)
                    .OrderByDescending(t => t.CreadoEn)
                    .ToListAsync();

    public async Task<Ticket?> GetWithDetallesAsync(int id) =>
        await _dbSet
            .Include(t => t.Estado)
            .Include(t => t.Prioridad)
            .Include(t => t.Categoria)
            .Include(t => t.Cliente)
            .Include(t => t.Tecnico)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<Ticket>> GetAllWithDetallesAsync() =>
        await _dbSet
        .Include(t => t.Estado)
        .Include(t => t.Prioridad)
        .Include(t => t.Categoria)
        .Include(t => t.Cliente)
        .Include(t => t.Tecnico)
        .OrderByDescending(t => t.CreadoEn)
        .ToListAsync();
}