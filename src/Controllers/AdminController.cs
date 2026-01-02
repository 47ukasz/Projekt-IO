using Microsoft.AspNetCore.Mvc;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("admin")]
public class AdminController : Controller {
    private readonly ILogger<AdminController> _logger;
    private readonly IUserService _userService;

    public AdminController(ILogger<AdminController> logger, IUserService userService) {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet("panel")]
    public async Task<IActionResult> Index() {
        var users = await _userService.GetAllUsersAsync();

        var viewModel = new AdminPanelViewModel() {
            Users = users
        };
        
        return View(viewModel);
    }
}