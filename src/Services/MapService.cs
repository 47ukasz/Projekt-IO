using projekt_io.Models;

namespace projekt_io.Services;

public class MapService : IMapService {
    private readonly ILogger<MapService> _logger;
    private readonly ILostReportService _lostReportService;

    public MapService(ILogger<MapService> logger, ILostReportService lostReportService) {
        _logger = logger;
        _lostReportService = lostReportService;
    }
    
    public async Task<List<MapPointViewModel>> GetMapPointsAsync() {
        var lostReports = await _lostReportService.GetAllLostReportsAsync();
        var mapPoints = new List<MapPointViewModel>();

        foreach (var lostReport in lostReports) {
            var point = new MapPointViewModel {
                Id = new Guid().ToString(),
                Latitude = lostReport.Location.Latitude,
                Longitude = lostReport.Location.Longitude,
                Status = lostReport.Status,
                Title = lostReport.Title,
                Type = "Lost"
            };
            
            mapPoints.Add(point);
        }
        
        return mapPoints;
    }
}