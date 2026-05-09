using TicketIT.API.DTOs.Request.Ticket;
using TicketIT.API.DTOs.Response.Ticket;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;
using TicketIT.API.Services.Interfaces;

namespace TicketIT.API.Services.Implementations;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepo;
    private readonly IAuditLogRepository _auditRepo;

    public TicketService(ITicketRepository ticketRepo, IAuditLogRepository auditRepo)
    {
        _ticketRepo = ticketRepo;
        _auditRepo = auditRepo;
    }

    public async Task<IEnumerable<TicketResponseDto>> GetAllAsync()
{
    var tickets = await _ticketRepo.GetAllWithDetallesAsync();
    return tickets.Select(MapToDto);
}

    public async Task<TicketResponseDto?> GetByIdAsync(int id)
    {
        var ticket = await _ticketRepo.GetWithDetallesAsync(id);
        return ticket is null ? null : MapToDto(ticket);
    }

    public async Task<IEnumerable<TicketResponseDto>> GetByClienteIdAsync(int clienteId)
    {
        var tickets = await _ticketRepo.GetByClienteIdAsync(clienteId);
        return tickets.Select(MapToDto);
    }

    public async Task<IEnumerable<TicketResponseDto>> GetByTecnicoIdAsync(int tecnicoId)
    {
        var tickets = await _ticketRepo.GetByTecnicoIdAsync(tecnicoId);
        return tickets.Select(MapToDto);
    }

    public async Task<TicketResponseDto> CrearAsync(CrearTicketRequestDto request, int clienteId)
    {
        var ticket = new Ticket
        {
            Titulo = request.Titulo,
            Descripcion = request.Descripcion,
            PrioridadId = request.PrioridadId,
            CategoriaId = request.CategoriaId,
            ClienteId = clienteId,
            EstadoId = 1, // -> activo
            CreadoEn = DateTime.UtcNow,
            ActualizadoEn = DateTime.UtcNow
        };

        var creado = await _ticketRepo.CreateAsync(ticket);

        await _auditRepo.RegisterAsync(creado.Id, clienteId, "estado_id", null, "Abierto");

        // Recarga con todas las relaciones
        var conDetalles = await _ticketRepo.GetWithDetallesAsync(creado.Id);
        return MapToDto(conDetalles!);
    }

    public async Task<TicketResponseDto?> ActualizarAsync(int id, ActualizarTicketRequestDto request, int changedBy)
    {
        var ticket = await _ticketRepo.GetWithDetallesAsync(id);
        if (ticket is null) return null;

        if (request.EstadoId.HasValue && request.EstadoId != ticket.EstadoId)
        {
            await _auditRepo.RegisterAsync(id, changedBy, "estado_id",
                ticket.Estado?.Nombre, request.EstadoId.ToString());
            ticket.EstadoId = request.EstadoId.Value;

            if (request.EstadoId == 5) // Cerrado
                ticket.CerradoEn = DateTime.UtcNow;
        }

        if (request.PrioridadId.HasValue && request.PrioridadId != ticket.PrioridadId)
        {
            await _auditRepo.RegisterAsync(id, changedBy, "prioridad_id",
                ticket.Prioridad?.Nombre, request.PrioridadId.ToString());
            ticket.PrioridadId = request.PrioridadId.Value;
        }

        if (request.TecnicoId.HasValue && request.TecnicoId != ticket.TecnicoId)
        {
            await _auditRepo.RegisterAsync(id, changedBy, "tecnico_id",
                ticket.TecnicoId?.ToString(), request.TecnicoId.ToString());
            ticket.TecnicoId = request.TecnicoId.Value;
        }

        ticket.ActualizadoEn = DateTime.UtcNow;
        await _ticketRepo.UpdateAsync(ticket);

        return MapToDto(ticket);
    }

    private static TicketResponseDto MapToDto(Ticket t) => new()
    {
        Id = t.Id,
        Titulo = t.Titulo,
        Descripcion = t.Descripcion,
        Estado = t.Estado?.Nombre ?? "",
        Prioridad = t.Prioridad?.Nombre ?? "",
        Categoria = t.Categoria?.Nombre,
        Cliente = t.Cliente?.Nombre ?? "",
        Tecnico = t.Tecnico?.Nombre,
        CreadoEn = t.CreadoEn ?? DateTime.UtcNow,
        CerradoEn = t.CerradoEn
    };
    
}