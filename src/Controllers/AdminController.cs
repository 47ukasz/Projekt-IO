using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Entities;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Authorize(Roles = "Admin")]
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

    [HttpPost("updateRoles")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRoles([FromForm] string id, [FromForm] List<string> roles) {
        var currentUser = await _userManager.GetUserAsync(User);
        var result = await _userService.ChangeUserRoles(id, currentUser.Id, roles);

        if (!result) {
            TempData["Error"] = "Nie udało się zmienić roli użytkownikowi.";
            return RedirectToAction("Index", "Profile", new {tab = "users"});
        }

        TempData["Success"] = "Role użytkownika zostały zaktualizowane.";
        return RedirectToAction("Index", "Profile", new {tab = "users"});

    }

    [HttpPost("block")]
    public async Task<IActionResult> BlockUser([FromForm] string id) {
        var result = await _userService.BlockUserAsync(id);

        if (!result) {
            TempData["Error"] = "Nie udało się zablokować użytkownika.";
            return RedirectToAction("Index", "Profile", new {tab = "users"});

        }

        TempData["Success"] = "Użytkownik został zablokowany.";
        return RedirectToAction("Index", "Profile", new {tab = "users"});

    }

    [HttpPost("unblock")]
    public async Task<IActionResult> UnblockUser([FromForm] string id) {
        var result = await _userService.UnblockUserAsync(id);
        
        if (!result) {
            TempData["Error"] = "Nie udało się odblokować użytkownika.";
            return RedirectToAction("Index", "Profile", new {tab = "users"});

        }

        TempData["Success"] = "Użytkownik został odblokowany.";
        return RedirectToAction("Index", "Profile", new {tab = "users"});
    }
}