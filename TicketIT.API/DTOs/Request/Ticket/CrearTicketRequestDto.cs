using System.ComponentModel.DataAnnotations;

namespace TicketIT.API.DTOs.Request.Ticket;

public class CrearTicketRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;

    [Required]
    public string Descripcion { get; set; } = null!;

    [Required]
    public int PrioridadId { get; set; }

    [Required]
    public int CategoriaId { get; set; }
}