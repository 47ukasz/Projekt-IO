using System.Globalization;
using Microsoft.AspNetCore.Authorization;
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
            Description = viewModel.Description.ToLower(),
            Location = locationDto,
            SeenDate = viewModel.SeenDate,
            SeenTime = viewModel.SeenTime,
        };
        
        var result = await _sightingService.CreateAsync(userId, sightingDto, viewModel.Photo);
        
        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie można dodać doniesienia.");
            return View("Form", viewModel);
        }
        
        return RedirectToAction("Index", "Map");
    }
    
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(string id) {
        var userId = _userManager.GetUserId(User);
        
        var sighting = await _sightingService.GetSightingByIdAsync(userId, id);

        if (sighting == null) {
            return NotFound();
        }

        var viewModel = new SightingFormViewModel() {
            IsEdit = true,
            Id = sighting.Id,
            LostReportId = sighting.LostReport.Id,
            Description = sighting.Description.ToLower(),
            SeenDate = sighting.SeenDate,
            Lat = sighting.Location.Latitude.ToString(CultureInfo.InvariantCulture),
            Lng = sighting.Location.Longitude.ToString(CultureInfo.InvariantCulture)
        };
        
        return View("Form", viewModel);
    }
    
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(string id, SightingFormViewModel viewModel) {
        viewModel.IsEdit = true;
        viewModel.Id = id;
        viewModel.LostReportId = viewModel.LostReportId;

        if (!ModelState.IsValid) {
            return View("Form", viewModel);
        }
        
        var locationDto = new LocationDto() {
            Latitude = float.Parse(viewModel.Lat, CultureInfo.InvariantCulture),
            Longitude = float.Parse(viewModel.Lng, CultureInfo.InvariantCulture) 
        };

        var sightingDto = new SightingDto() {
            Id = viewModel.Id,
            LostReportId = viewModel.LostReportId,
            Description = viewModel.Description.ToLower(),
            Location = locationDto,
            SeenDate = viewModel.SeenDate,
        };
        
        var userId = _userManager.GetUserId(User);
        
        var result = await _sightingService.UpdateAsync(userId, sightingDto, viewModel.Photo);

        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie udało się edytować doniesienia.");
            return View("Form", viewModel);
        }
        
        return RedirectToAction("Index", "Map");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("delete")]
    public async Task<IActionResult> Delete(string id, string? userId) {
        var result = await _sightingService.DeleteAsync(id);

        if (!result) {
            TempData["Error"] = "Nie udało się usunąć doniesienia.";
            return RedirectToAction("UserProfile", "Profile", new {userId, tab = "sights"});
        }

        TempData["Success"] = "Doniesienie zostało usunięte.";
        return RedirectToAction("UserProfile", "Profile", new {userId, tab = "sights"});
    }
}