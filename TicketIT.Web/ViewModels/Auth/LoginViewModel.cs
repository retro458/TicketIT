using System.ComponentModel.DataAnnotations;

namespace TicketIT.Web.ViewModels.Auth;

public class LoginViewModel
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6)]
    public string Password { get; set; } = null!;
}