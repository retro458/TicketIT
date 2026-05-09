using System.ComponentModel.DataAnnotations;

namespace TicketIT.API.DTOs.Request.Auth;

public class RegisterRequestDto
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    public int RolId { get; set; }
}