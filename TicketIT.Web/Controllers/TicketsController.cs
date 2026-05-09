using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketIT.Web.Services;
using TicketIT.Web.ViewModels.Tickets;
using TicketIT.Web.ViewModels;

namespace TicketIT.Web.Controllers;

[Authorize]
public class TicketsController : Controller
{
    private readonly ApiService _apiService;

    public TicketsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Index()
    {
        var tickets = await _apiService.GetAsync<List<TicketViewModel>>("api/tickets");
        return View(tickets ?? new List<TicketViewModel>());
    }

    public async Task<IActionResult> MisTickets()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var rol = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        List<TicketViewModel>? tickets;

        if (rol == "Tecnico")
            tickets = await _apiService.GetAsync<List<TicketViewModel>>($"api/tickets/tecnico/{userId}");
        else
            tickets = await _apiService.GetAsync<List<TicketViewModel>>($"api/tickets/cliente/{userId}");

        return View(tickets ?? new List<TicketViewModel>());
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var ticket = await _apiService.GetAsync<TicketViewModel>($"api/tickets/{id}");
        if (ticket is null) return NotFound();
        return View(ticket);
    }

    [Authorize(Roles = "Cliente,Administrador")]
    public async Task<IActionResult> Crear()
    {
        var model = await CargarSelectLists(new CrearTicketViewModel());
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Cliente,Administrador")]
    public async Task<IActionResult> Crear(CrearTicketViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await CargarSelectLists(model));

        var (success, _, error) = await _apiService.PostAsync<TicketViewModel>("api/tickets", new
        {
            model.Titulo,
            model.Descripcion,
            model.PrioridadId,
            model.CategoriaId
        });

        if (!success)
        {
            ModelState.AddModelError("", "Error al crear el ticket.");
            return View(await CargarSelectLists(model));
        }

        return RedirectToAction(nameof(MisTickets));
    }

    private async Task<CrearTicketViewModel> CargarSelectLists(CrearTicketViewModel model)
    {
        var prioridades = await _apiService.GetAsync<List<CatalogoViewModel>>("api/catalogos/prioridades");
        var categorias = await _apiService.GetAsync<List<CatalogoViewModel>>("api/catalogos/categorias");

        model.Prioridades = prioridades?.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = p.Nombre
        }).ToList() ?? new();

        model.Categorias = categorias?.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Nombre
        }).ToList() ?? new();

        return model;
    }
}