using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.DTOs;
using projekt_io.Entities;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("report")]
public class LostReportController : Controller {
    private readonly ILogger<LostReportController> _logger;
    private readonly ILostReportService _lostReportService;
    private readonly UserManager<ApplicationUser> _userManager;

    public LostReportController(ILogger<LostReportController> logger, ILostReportService lostReportService, UserManager<ApplicationUser> userManager) {
        _logger = logger;
        _lostReportService = lostReportService;
        _userManager = userManager;
    }
    
    [HttpGet("create")]
    public IActionResult Create() {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(LostReportFormViewModel model) {
        if (!ModelState.IsValid) {
            return View(model);
        }

        var animalDto = new AnimalDto() {
            Name = model.Name,
            Species = model.Species,
            Breed = model.Breed,
            Description = model.Description,
        };

        var locationDto = new LocationDto() {
            Latitude = float.Parse(model.Lat, CultureInfo.InvariantCulture),
            Longitude = float.Parse(model.Lng, CultureInfo.InvariantCulture) 
        };

        var lostReportDto = new LostReportDto() {
            Animal = animalDto,
            Location = locationDto,
            CreatedAt = DateOnly.FromDateTime(DateTime.Today),
            UpdatedAt = DateOnly.FromDateTime(DateTime.MaxValue),
            LostAt = DateOnly.FromDateTime(model.MissingDate!.Value),
            Status = "lost",
            Title = model.Title,
        };

        var userId = _userManager.GetUserId(User);
        var result = await _lostReportService.CreateAsync(userId, lostReportDto, model.Photo);

        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie można dodać zgłoszenia o zaginięciu.");
            return View(model);
        }
        
        return RedirectToAction("Index", "Map");
    }
}