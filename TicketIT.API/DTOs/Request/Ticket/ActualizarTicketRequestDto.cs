namespace TicketIT.API.DTOs.Request.Ticket;

public class ActualizarTicketRequestDto
{
    public int? EstadoId { get; set; }
    public int? PrioridadId { get; set; }
    public int? TecnicoId { get; set; }
}