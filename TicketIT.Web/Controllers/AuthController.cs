using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using TicketIT.Web.Services;
using TicketIT.Web.ViewModels.Auth;

namespace TicketIT.Web.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _apiService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(ApiService apiService, IHttpContextAccessor httpContextAccessor)
    {
        _apiService = apiService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (success, data, error) = await _apiService.PostAsync<LoginResponseViewModel>(
            "api/auth/login", new { model.Email, model.Password });

        if (!success)
        {
            ModelState.AddModelError("", "Credenciales incorrectas.");
            return View(model);
        }

        // Guarda el token en Session (HttpOnly, no accesible desde JS)
        HttpContext.Session.SetString("JwtToken", data!.Token);

        // Crea la cookie de autenticación
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, data.Nombre),
            new Claim(ClaimTypes.Email, data.Email),
            new Claim(ClaimTypes.Role, data.Rol)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = true });

        return data.Rol switch
        {
            "Administrador" => RedirectToAction("Index", "Dashboard"),
            "Tecnico" => RedirectToAction("MisTickets", "Tickets"),
            _ => RedirectToAction("MisTickets", "Tickets")
        };
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        HttpContext.Session.Clear();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult AccesoDenegado() => View();
}