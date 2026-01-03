using Microsoft.AspNetCore.Mvc;

namespace projekt_io.Controllers;

[Route("map")]
public class MapController : Controller{
    private readonly ILogger<MapController> _logger;

    public MapController(ILogger<MapController> logger) {
        _logger = logger;
    }
    
    [HttpGet("")]
    public IActionResult Index() {
        return View();
    }
}