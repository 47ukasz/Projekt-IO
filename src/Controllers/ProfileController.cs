using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Entities;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("profile")]
public class ProfileController : Controller {
    private readonly ILogger<ProfileController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILostReportService _lostReportService;
    private readonly IUserService _userService;
    
    public ProfileController(ILogger<ProfileController> logger, UserManager<ApplicationUser> userManager, ILostReportService lostReportService, IUserService userService) {
        _logger = logger;
        _userManager = userManager;
        _lostReportService = lostReportService;
        _userService = userService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string tab = "reports") {
        var userId = _userManager.GetUserId(User);
        var userReports = await _lostReportService.GetLostReportsByIdAsync(userId);
        var userDto = await _userService.GetUserById(userId);
        
        var viewModel = new ProfileViewModel() {
            CurrentTab = tab,
            Reports = userReports,
            UserFullName = userDto.FirstName + " " + userDto.LastName,
            UserEmail = userDto.Email,
        };
        
        return View(viewModel);
    }
}