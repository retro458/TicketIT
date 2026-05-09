using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketIT.Web.ViewModels.Tickets;

public class CrearTicketViewModel
{
    [Required(ErrorMessage = "El título es requerido")]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es requerida")]
    public string Descripcion { get; set; } = null!;

    [Required]
    public int PrioridadId { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    public List<SelectListItem> Prioridades { get; set; } = new();
    public List<SelectListItem> Categorias { get; set; } = new();
}