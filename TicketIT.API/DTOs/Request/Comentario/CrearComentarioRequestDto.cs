using System.ComponentModel.DataAnnotations;

namespace TicketIT.API.DTOs.Request.Comentario;

public class CrearComentarioRequestDto
{
    [Required]
    public int TicketId { get; set; }

    [Required]
    public string Contenido { get; set; } = null!;

    [Required]
    public int TipoId { get; set; }
}