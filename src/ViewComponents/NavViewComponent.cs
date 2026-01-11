using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Entities;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.ViewComponents;

public class NavViewComponent : ViewComponent {
    public readonly UserManager<ApplicationUser> _userManager;
    public readonly IUserService _userService;
    
    public NavViewComponent(UserManager<ApplicationUser> userManager, IUserService userService) {
        _userManager = userManager;
        _userService = userService;
    }
    
    public async Task<IViewComponentResult> InvokeAsync(string variant = "mobile") {
        var userId = _userManager.GetUserId(HttpContext.User);
        var userDto = await _userService.GetUserById(userId);
        var isAdmin = userDto?.Roles.Contains("Admin") ?? false;
        var isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
        List<(string, string)> navItems = new List<(string, string)>();
        
        navItems.Add(("Mapa", "/map"));
        
        if (isAuthenticated) {
            navItems.Add(("Dodaj zgłoszenie", "/report/create"));
            navItems.Add(("Dodaj doniesienie", "/"));
        }

        if (isAdmin) {
            navItems.Add(("Panel administratora", "/profile?tab=users"));
        }

        var viewModel = new NavViewModel() {
            IsAdmin = isAdmin,
            NavItems = navItems,
            IsAuthenticated = isAuthenticated
        };
        
        return View(variant, viewModel);
    }
}