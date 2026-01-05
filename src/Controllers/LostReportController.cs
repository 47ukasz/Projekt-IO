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
        var viewModel = new LostReportFormViewModel() {
            IsEdit = false
        };
        
        return View("Form", viewModel);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(string id) {
        var userId = _userManager.GetUserId(User);
        
        var report = await _lostReportService.GetLostReportByIdAsync(userId, id);

        if (report == null) {
            return NotFound();
        }
        
        var viewModel = new LostReportFormViewModel() {
            IsEdit = true,
            Id = report.Id,
            Name = report.Animal.Name,
            Species = report.Animal.Species,
            Breed = report.Animal?.Breed ?? "",
            Description = report.Animal.Description,
            Title = report.Title,
            MissingDate = report.LostAt,
            Lat = report.Location.Latitude.ToString(CultureInfo.InvariantCulture),
            Lng = report.Location.Longitude.ToString(CultureInfo.InvariantCulture)
        };
        
        Console.WriteLine($"Lat: {viewModel.Lat}, Lng: {viewModel.Lng}");
        
        return View("Form", viewModel);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(string id, LostReportFormViewModel viewModel) {
        viewModel.IsEdit = true;
        viewModel.Id = id;
        
        if (!ModelState.IsValid) {
            return View("Form", viewModel);
        }
        
        var animalDto = new AnimalDto() {
            Name = viewModel.Name,
            Species = viewModel.Species,
            Breed = viewModel.Breed,
            Description = viewModel.Description,
        };

        var locationDto = new LocationDto() {
            Latitude = float.Parse(viewModel.Lat, CultureInfo.InvariantCulture),
            Longitude = float.Parse(viewModel.Lng, CultureInfo.InvariantCulture) 
        };

        var lostReportDto = new LostReportDto() {
            Id = viewModel.Id,
            Animal = animalDto,
            Location = locationDto,
            LostAt = viewModel.MissingDate,
            Status = "Zaginiony",
            Title = viewModel.Title,
        };
        
        var userId = _userManager.GetUserId(User);
        
        var result = await _lostReportService.UpdateAsync(userId, lostReportDto, viewModel.Photo);
        
        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie udało się edytować zgłoszenia.");
            return View("Form", viewModel);
        }
        
        return RedirectToAction("Index", "Profile");
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(LostReportFormViewModel model) {
        if (!ModelState.IsValid) {
            return View("Form", model);
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
            LostAt = model.MissingDate,
            Status = "lost",
            Title = model.Title,
        };

        var userId = _userManager.GetUserId(User);
        var result = await _lostReportService.CreateAsync(userId, lostReportDto, model.Photo);

        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie można dodać zgłoszenia o zaginięciu.");
            return View("Form", model);
        }
        
        return RedirectToAction("Index", "Map");
    }
}