using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketIT.API.DTOs.Request.Auth;
using TicketIT.API.DTOs.Response.Auth;
using TicketIT.API.Models;
using TicketIT.API.Repositories.Interfaces;
using TicketIT.API.Services.Interfaces;

namespace TicketIT.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IConfiguration _config;

    public AuthService(IUsuarioRepository usuarioRepo, IConfiguration config)
    {
        _usuarioRepo = usuarioRepo;
        _config = config;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _usuarioRepo.GetByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Credenciales incorrectas.");

        if (usuario.Activo == false)
            throw new UnauthorizedAccessException("Usuario inactivo.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales incorrectas.");

        return GenerarToken(usuario);
    }
public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request)
{
    if (await _usuarioRepo.ExistsEmailAsync(request.Email))
        throw new InvalidOperationException("El email ya está registrado.");

    var usuario = new Usuario
    {
        Nombre = request.Nombre,
        Email = request.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        RolId = request.RolId,
        Provider = "local",
        Activo = true,
        CreadoEn = DateTime.UtcNow
    };

    await _usuarioRepo.CreateAsync(usuario);
    
    // Recarga con el rol incluido
    var usuarioConRol = await _usuarioRepo.GetByEmailAsync(request.Email);
    return GenerarToken(usuarioConRol!);
}

    private LoginResponseDto GenerarToken(Usuario usuario)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var expiracion = DateTime.UtcNow.AddHours(int.Parse(jwtSettings["ExpirationHours"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "Cliente")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiracion,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new LoginResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol?.Nombre ?? "Cliente",
            Expiracion = expiracion
        };
    }
}