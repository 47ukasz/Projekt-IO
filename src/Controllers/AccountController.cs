using Microsoft.AspNetCore.Mvc;
using projekt_io.DTOs.Auth;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("")]
public class AccountController : Controller {
    private readonly ILogger<AccountController> _logger;
    private readonly IAuthService _authService;
    
    public AccountController(ILogger<AccountController> logger, IAuthService authService) {
        _logger = logger;
        _authService = authService;
    }

    [HttpGet("register")]
    public IActionResult Register() {
        return View();
    }
    
    [HttpGet("login")]
    public IActionResult Login() {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model) {
        if (!ModelState.IsValid) {
            return View(model);
        }

        var result = await _authService.LoginAsync(new LoginDto() {
            Email = model.Email,
            Password = model.Password
        });

        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie można zalogować się.");
            return View(model);
        }
        
        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model) {
        if (!ModelState.IsValid) {
            return View(model);
        }
        
        var result = await _authService.RegisterAsync(new RegisterDto() {
            Email = model.Email,
            Password = model.Password,
            FirstName = model.FirstName,
            LastName = model.SecondName,
        });

        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie można stworzyć nowego konta.");
            return View(model);
        }
        
        return RedirectToAction("Login", "Account");
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout() {
        await _authService.LogoutAsync();
        return RedirectToAction("Login", "Account");
    }
}