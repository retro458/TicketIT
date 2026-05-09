using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketIT.Web.Services;
using TicketIT.Web.ViewModels.Tickets;

namespace TicketIT.Web.Controllers;

[Authorize(Roles = "Administrador")]
public class DashboardController : Controller
{
    private readonly ApiService _apiService;

    public DashboardController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var tickets = await _apiService.GetAsync<List<TicketViewModel>>("api/tickets");
        return View(tickets ?? new List<TicketViewModel>());
    }
}