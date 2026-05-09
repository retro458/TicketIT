using TicketIT.API.Models;

namespace TicketIT.API.Repositories.Interfaces;

public interface ITicketRepository : IGenericRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetByClienteIdAsync(int clienteId);
    Task<IEnumerable<Ticket>> GetByTecnicoIdAsync(int tecnicoId);
    Task<IEnumerable<Ticket>> GetByEstadoIdAsync(int estadoId);
    Task<IEnumerable<Ticket>> GetByPrioridadIdAsync(int prioridadId);
    Task<Ticket?> GetWithDetallesAsync(int id); // incluye usuario, estado, prioridad, categoria
    Task<IEnumerable<Ticket>> GetAllWithDetallesAsync();
}