namespace TicketIT.Web.ViewModels.Chat;

public class MensajeChatViewModel
{
    public int Id { get; set; }
    public string Contenido { get; set; } = null!;
    public string Emisor { get; set; } = null!;
    public int EmisorId { get; set; }
    public string TipoMensaje { get; set; } = null!;
    public bool Leido { get; set; }
    public bool EsPrivado { get; set; }
    public DateTime CreadoEn { get; set; }
}