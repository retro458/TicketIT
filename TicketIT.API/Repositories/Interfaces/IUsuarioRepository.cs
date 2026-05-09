using TicketIT.API.Models;

namespace TicketIT.API.Repositories.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByExternalIdAsync(string externalId, string provider);
    Task<IEnumerable<Usuario>> GetByRolIdAsync(int rolId);
    Task<bool> ExistsEmailAsync(string email);
}