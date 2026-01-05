using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.DTOs;
using projekt_io.Entities;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("sighting")]
public class SightingController : Controller{
    private readonly ILogger<SightingController> _logger;
    private readonly ISightingService _sightingService;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public SightingController(ILogger<SightingController> logger, UserManager<ApplicationUser> userManager, ISightingService sightingService) {
        _logger = logger;
        _userManager = userManager;
        _sightingService = sightingService;
    }
    
    [HttpGet("create/{id}")]
    public IActionResult Create(string id) {
        if (String.IsNullOrWhiteSpace(id)) {
            return NotFound();
        }

        var viewModel = new SightingFormViewModel() {
            IsEdit = false,
            LostReportId = id
        };
        
        return View("Form", viewModel);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(SightingFormViewModel viewModel) {
        Console.WriteLine("Wykonano");
        
        if (!ModelState.IsValid) {
            return View("Form", viewModel);
        }
        
        var userId = _userManager.GetUserId(User);
        
        var locationDto = new LocationDto() {
            Latitude = float.Parse(viewModel.Lat, CultureInfo.InvariantCulture),
            Longitude = float.Parse(viewModel.Lng, CultureInfo.InvariantCulture) 
        };

        var sightingDto = new SightingDto() {
            LostReportId = viewModel.LostReportId,
            Description = viewModel.Description,
            Location = locationDto,
            SeenDate = viewModel.SeenDate,
        };
        
        var result = await _sightingService.CreateAsync(userId, sightingDto, viewModel.Photo);
        
        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie można dodać doniesienia.");
            return View("Form", viewModel);
        }
        
        return RedirectToAction("Index", "Map");
    }
}