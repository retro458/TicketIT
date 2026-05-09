namespace TicketIT.API.DTOs.Response.AuditLog;

public class AuditLogResponseDto
{
    public int Id { get; set; }
    public string CambiadoPor { get; set; } = null!;
    public string Campo { get; set; } = null!;
    public string? ValorAnterior { get; set; }
    public string? ValorNuevo { get; set; }
    public DateTime Fecha { get; set; }
}