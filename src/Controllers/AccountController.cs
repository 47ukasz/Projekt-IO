using Microsoft.AspNetCore.Mvc;
using projekt_io.DTOs.Auth;
using projekt_io.Models;
using projekt_io.Results;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("")]
public class AccountController : Controller {
    private readonly ILogger<AccountController> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    
    public AccountController(ILogger<AccountController> logger, IAuthService authService, IEmailService emailService) {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
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

        var (user, tokenEncoded) = await _authService.GenerateConfirmationToken(model.Email);

        if (tokenEncoded == String.Empty || user == null) {
            ModelState.AddModelError(string.Empty, "Nie można wygenerować linku weryfikacyjnego.");
            return View(model);
        }
        
        var callbackUrl = Url.Action(
            action: "Activate",
            controller: "Account",
            values: new { userId = user.Id, token = tokenEncoded },
            protocol: Request.Scheme
        );
        
        _emailService.SendConfirmationEmail(user, callbackUrl);
        
        return RedirectToAction("Login", "Account");
    }

    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout() {
        await _authService.LogoutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet("activate")]
    public async Task<IActionResult> Activate(string userId, string token) {
        var result = await _authService.ActivateAccountAsync(userId, token);

        if (result == ActivateAccountResult.InvalidOrExpired) {
            TempData["Error"] = "Podany link weryfikacyjny jest niepoprawny.";
            return RedirectToAction("Register");
        }  
        
        if (result == ActivateAccountResult.AlreadyActivated) {
            TempData["Info"] = "Konto jest aktywne. Możesz się zalogować.";
            return RedirectToAction("Login");
        }
        
        TempData["Success"] = "Konto zostało aktywowane. Możesz się zalogować.";
        return RedirectToAction("Login");
    }
}