using projekt_io.Models;

namespace projekt_io.Services;

public class MapService : IMapService {
    private readonly ILogger<MapService> _logger;
    private readonly ILostReportService _lostReportService;
    private readonly ISightingService _sightingService;

    public MapService(ILogger<MapService> logger, ILostReportService lostReportService, ISightingService sightingService) {
        _logger = logger;
        _lostReportService = lostReportService;
        _sightingService = sightingService;
    }
    
    public async Task<List<MapPointViewModel>> GetMapPointsAsync(MapFilterViewModel? filter = null) {
        var lostReports = await _lostReportService.GetAllLostReportsAsync();
        var sightings = await _sightingService.GetAllSightingsAsync();
        var mapPoints = new List<MapPointViewModel>();

        var reportSpeciesDictionary = new Dictionary<string, string>();
        
        foreach (var lostReport in lostReports) {
            var point = new MapPointViewModel {
                LostReportId = lostReport.Id,
                Latitude = lostReport.Location.Latitude,
                Longitude = lostReport.Location.Longitude,
                Status = lostReport.Status,
                Species = lostReport.Animal.Species,
                Type = "zgłoszenie",
                PhotoPath = lostReport.Animal.PhotoPath,
            };
            
            reportSpeciesDictionary.Add(lostReport.Id, lostReport.Animal.Species);
            
            mapPoints.Add(point);
        }

        foreach (var sighting in sightings) {
            var point = new MapPointViewModel() {
                SightingId = sighting.Id,
                LostReportId = sighting.LostReport.Id,
                Latitude = sighting.Location.Latitude,
                Longitude = sighting.Location.Longitude,
                Type = "doniesienie",
                Status = sighting.LostReport.Status,
                PhotoPath = sighting.PhotoPath,
                Species = reportSpeciesDictionary[sighting.LostReport.Id],
            };
            
            mapPoints.Add(point);
        }

        if (filter == null) {
            return mapPoints;
        }

        if (!string.IsNullOrWhiteSpace(filter.Type)) {
            mapPoints = mapPoints.Where(p => p.Type.ToLower() == filter.Type.ToLower()).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status)) {
            mapPoints = mapPoints.Where(p => p.Status.ToLower() == filter.Status.ToLower()).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Species)) {
            mapPoints = mapPoints.Where(p => p.Species.ToLower().Contains(filter.Species.ToLower())).ToList();
        }
        
        return mapPoints;
    }
}