using TicketIT.API.DTOs.Request.Auth;
using TicketIT.API.DTOs.Response.Auth;

namespace TicketIT.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto request);
}