namespace TicketIT.Web.ViewModels.Auth;

public class LoginResponseViewModel
{
    public string Token { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public DateTime Expiracion { get; set; }
}