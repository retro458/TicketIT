using Microsoft.EntityFrameworkCore;
using TicketIT.API.Data;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;

namespace TicketIT.API.Repositories.Implementations;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AppDbContext _context;

    public AuditLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AuditLog>> GetByTicketIdAsync(int ticketId) =>
        await _context.AuditLogs
                      .Where(a => a.TicketId == ticketId)
                      .Include(a => a.ChangedByNavigation)
                      .OrderByDescending(a => a.CreatedAt)
                      .ToListAsync();

    public async Task RegisterAsync(int ticketId, int changedBy, string fieldChanged, string? oldValue, string? newValue)
    {
        var log = new AuditLog
        {
            TicketId = ticketId,
            ChangedBy = changedBy,
            FieldChanged = fieldChanged,
            OldValue = oldValue,
            NewValue = newValue,
            CreatedAt = DateTime.UtcNow
        };

        await _context.AuditLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}