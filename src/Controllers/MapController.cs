using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("map")]
public class MapController : Controller{
    private readonly ILogger<MapController> _logger;
    private readonly IMapService _mapService;
    
    public MapController(ILogger<MapController> logger, IMapService mapService) {
        _logger = logger;
        _mapService = mapService;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> Index() {
        var points = await _mapService.GetMapPointsAsync();
        
        ViewBag.MapPointsJson = JsonSerializer.Serialize(points);
        
        return View();
    }
}