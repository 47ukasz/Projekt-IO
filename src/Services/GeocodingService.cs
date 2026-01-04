using System.Globalization;
using System.Text.Json;

namespace projekt_io.Services;

public class GeocodingService : IGeocodingService {
    private readonly ILogger<GeocodingService> _logger;
    private readonly HttpClient _httpClient;
    
    public GeocodingService(ILogger<GeocodingService> logger, IHttpClientFactory factory) {
        _logger = logger;
        _httpClient = factory.CreateClient("Nominatim");
    }
    
    public async Task<string?> GetCityAsync(float lat, float lon) {
        var latStr = lat.ToString(CultureInfo.InvariantCulture);
        var lonStr = lon.ToString(CultureInfo.InvariantCulture);
        
        var url = $"reverse?format=json&lat={latStr}&lon={lonStr}&addressdetails=1";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode) {
            return null;
        }
        
        var json = await response.Content.ReadAsStringAsync();
        
        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("address", out var address)) {
            return null;
        }

        if (address.TryGetProperty("city", out var city)) {
            return city.GetString();
        }

        if (address.TryGetProperty("town", out var town)) {
            return town.GetString();
        }

        if (address.TryGetProperty("village", out var village)) {
            return village.GetString();
        }

        if (address.TryGetProperty("municipality", out var municipality)) {
            return municipality.GetString();
        }

        return null;

    }
}