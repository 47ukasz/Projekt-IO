using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("map")]
public class MapController : Controller{
    private readonly ILogger<MapController> _logger;
    private readonly IMapService _mapService;
    private readonly IGeocodingService _geocodingService;
    
    public MapController(ILogger<MapController> logger, IMapService mapService, IGeocodingService geocodingService) {
        _logger = logger;
        _mapService = mapService;
        _geocodingService = geocodingService;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> Index([FromQuery] MapFilterViewModel filter) {
        var points = await _mapService.GetMapPointsAsync(filter);
        
        (float lat, float lon)? cords = null;
        var location = filter.Location?.Trim();
        
        if (!string.IsNullOrWhiteSpace(location)) {
            cords = await _geocodingService.GeocodeAsync(location.ToLowerInvariant());

            if (cords != null) {
                ViewBag.CordinatesJson = JsonSerializer.Serialize(new { lat = cords.Value.lat, lon = cords.Value.lon });
            }
        }
        
        ViewBag.MapPointsJson = JsonSerializer.Serialize(points);
        
        return View(filter);
    }
}