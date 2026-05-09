using System.ComponentModel.DataAnnotations;

namespace TicketIT.API.DTOs.Request.Chat;

public class EnviarMensajeRequestDto
{
    [Required]
    public int TicketId { get; set; }

    [Required]
    public string Contenido { get; set; } = null!;

    public string TipoMensaje { get; set; } = "text"; // text, file

    public bool EsPrivado { get; set; } = false;
}