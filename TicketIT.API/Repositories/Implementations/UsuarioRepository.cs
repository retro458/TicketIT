using Microsoft.EntityFrameworkCore;
using TicketIT.API.Data;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;

namespace TicketIT.API.Repositories.Implementations;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) { }

    public async Task<Usuario?> GetByEmailAsync(string email) =>
        await _dbSet.Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<Usuario?> GetByExternalIdAsync(string externalId, string provider) =>
        await _dbSet.FirstOrDefaultAsync(u => u.ExternalId == externalId && u.Provider == provider);

    public async Task<IEnumerable<Usuario>> GetByRolIdAsync(int rolId) =>
        await _dbSet.Where(u => u.RolId == rolId && u.Activo == true)
                    .ToListAsync();

    public async Task<bool> ExistsEmailAsync(string email) =>
        await _dbSet.AnyAsync(u => u.Email == email);
}