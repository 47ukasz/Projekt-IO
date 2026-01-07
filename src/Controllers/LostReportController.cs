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
    private readonly ISightingService _sightingService;

    public LostReportController(ILogger<LostReportController> logger, ILostReportService lostReportService, UserManager<ApplicationUser> userManager, ISightingService sightingService) {
        _logger = logger;
        _lostReportService = lostReportService;
        _userManager = userManager;
        _sightingService = sightingService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(string id) {
        var lostReport = await _lostReportService.GetLostReportByIdAsync(id);

        if (lostReport == null) {
            return NotFound();
        }

        var sightings = await _sightingService.GetSightingsByLostReportIdAsync(id);

        var viewModel = new LostReportPageViewModel() {
            LostReport = lostReport,
            SightingsComments = sightings
        };
        
        return View(viewModel);
    }
    
    [HttpGet("create")]
    public IActionResult Create() {
        var viewModel = new LostReportFormViewModel() {
            IsEdit = false
        };
        
        return View("Form", viewModel);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(LostReportFormViewModel model) {
        if (!ModelState.IsValid) {
            return View("Form", model);
        }

        var animalDto = new AnimalDto() {
            Name = model.Name.ToLower(),
            Species = model.Species.ToLower(),
            Breed = model.Breed.ToLower(),
            Description = model.Description.ToLower(),
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
            Status = "zaginiony",
            Title = model.Title.ToLower(),
        };

        var userId = _userManager.GetUserId(User);
        var result = await _lostReportService.CreateAsync(userId, lostReportDto, model.Photo);

        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie można dodać zgłoszenia o zaginięciu.");
            return View("Form", model);
        }
        
        return RedirectToAction("Index", "Map");
    }
    
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(string id) {
        var report = await _lostReportService.GetLostReportByIdAsync(id);

        if (report == null) {
            return NotFound();
        }
        
        var viewModel = new LostReportFormViewModel() {
            IsEdit = true,
            Id = report.Id,
            Name = report.Animal.Name.ToLower(),
            Species = report.Animal.Species.ToLower(),
            Breed = report.Animal?.Breed.ToLower() ?? "",
            Description = report.Animal.Description.ToLower(),
            Title = report.Title.ToLower(),
            MissingDate = report.LostAt,
            Lat = report.Location.Latitude.ToString(CultureInfo.InvariantCulture),
            Lng = report.Location.Longitude.ToString(CultureInfo.InvariantCulture)
        };
        
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
            Name = viewModel.Name.ToLower(),
            Species = viewModel.Species.ToLower(),
            Breed = viewModel.Breed.ToLower(),
            Description = viewModel.Description.ToLower(),
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
            Status = "zaginiony",
            Title = viewModel.Title.ToLower(),
        };
        
        var userId = _userManager.GetUserId(User);
        
        var result = await _lostReportService.UpdateAsync(userId, lostReportDto, viewModel.Photo);
        
        if (!result) {
            ModelState.AddModelError(string.Empty, "Nie udało się edytować zgłoszenia.");
            return View("Form", viewModel);
        }
        
        return RedirectToAction("Index", "Profile");
    }
}