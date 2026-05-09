using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketIT.API.Repositories.Interfaces;

namespace TicketIT.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Tecnico,Administrador")]
public class AuditLogController : ControllerBase
{
    private readonly IAuditLogRepository _auditRepo;

    public AuditLogController(IAuditLogRepository auditRepo)
    {
        _auditRepo = auditRepo;
    }

    [HttpGet("{ticketId}")]
    public async Task<IActionResult> GetByTicket(int ticketId) =>
        Ok(await _auditRepo.GetByTicketIdAsync(ticketId));
}