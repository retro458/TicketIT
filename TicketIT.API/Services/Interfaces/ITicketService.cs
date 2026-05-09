using TicketIT.API.DTOs.Request.Ticket;
using TicketIT.API.DTOs.Response.Ticket;

namespace TicketIT.API.Services.Interfaces;

public interface ITicketService
{
    Task<IEnumerable<TicketResponseDto>> GetAllAsync();
    Task<TicketResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<TicketResponseDto>> GetByClienteIdAsync(int clienteId);
    Task<IEnumerable<TicketResponseDto>> GetByTecnicoIdAsync(int tecnicoId);
    Task<TicketResponseDto> CrearAsync(CrearTicketRequestDto request, int clienteId);
    Task<TicketResponseDto?> ActualizarAsync(int id, ActualizarTicketRequestDto request, int changedBy);
}