namespace TicketIT.API.DTOs.Response.Comentario;

public class ComentarioResponseDto
{
    public int Id { get; set; }
    public string Contenido { get; set; } = null!;
    public string Usuario { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public DateTime CreadoEn { get; set; }
}