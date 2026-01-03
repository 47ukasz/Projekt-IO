using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Entities;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("admin")]
public class AdminController : Controller {
    private readonly ILogger<AdminController> _logger;
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ILogger<AdminController> logger, IUserService userService, UserManager<ApplicationUser> userManager) {
        _logger = logger;
        _userService = userService;
        _userManager = userManager;
    }

    [HttpGet("panel")]
    public async Task<IActionResult> Index() {
        var users = await _userService.GetAllUsersAsync();

        var viewModel = new AdminPanelViewModel() {
            Users = users
        };
        
        return View(viewModel);
    }

    [HttpPost("updateRoles")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRoles([FromForm] string userId, [FromForm] List<string> roles) {
        var currentUser = await _userManager.GetUserAsync(User);
        var result = await _userService.ChangeUserRoles(userId, currentUser.Id, roles);

        if (!result) {
            TempData["Error"] = "Nie udało się zmienić roli użytkownikowi.";
            return RedirectToAction("Index");
        }

        TempData["Success"] = "Role użytkownika zostały zaktualizowane.";
        return RedirectToAction("Index");
    }

    [HttpPost("block")]
    public async Task<IActionResult> BlockUser([FromForm] string userId) {
        var result = await _userService.BlockUserAsync(userId);

        if (!result) {
            TempData["Error"] = "Nie udało się zablokować użytkownika.";
            return RedirectToAction("Index");
        }

        TempData["Success"] = "Użytkownik został zablokowany.";
        return RedirectToAction("Index");
    }
}