using Microsoft.AspNetCore.Mvc;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("profile")]
public class ProfileController : Controller {
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(ILogger<ProfileController> logger) {
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index(string tab = "reports") {
        var viewModel = new ProfileViewModel() {
            CurrentTab = tab
        };
        
        return View(viewModel);
    }
}