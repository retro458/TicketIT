namespace TicketIT.Web.ViewModels.Tickets;

public class TicketViewModel
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public string Prioridad { get; set; } = null!;
    public string? Categoria { get; set; }
    public string Cliente { get; set; } = null!;
    public string? Tecnico { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? CerradoEn { get; set; }
}