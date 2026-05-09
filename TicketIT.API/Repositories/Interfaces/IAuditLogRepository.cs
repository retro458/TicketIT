using TicketIT.API.Models;

namespace TicketIT.API.Repositories.Interfaces;

public interface IAuditLogRepository
{
    Task<IEnumerable<AuditLog>> GetByTicketIdAsync(int ticketId);
    Task RegisterAsync(int ticketId, int changedBy, string fieldChanged, string? oldValue, string? newValue);
}