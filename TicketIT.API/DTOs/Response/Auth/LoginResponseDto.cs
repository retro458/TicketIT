namespace TicketIT.API.DTOs.Response.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public DateTime Expiracion { get; set; }
}