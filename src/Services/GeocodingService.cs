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

    public async Task<(float lat, float lon)?> GeocodeAsync(string cityName) {
        if (string.IsNullOrWhiteSpace(cityName)) {
            return null;
        }
        
        var url = $"search?format=json&q={Uri.EscapeDataString(cityName)}&limit=1";
        
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode) {
            return null;
        }
        
        var json = await response.Content.ReadAsStringAsync();
        
        using var doc = JsonDocument.Parse(json);
        
        var item = doc.RootElement[0];
        
        
        if (!item.TryGetProperty("lat", out var latProp) || !item.TryGetProperty("lon", out var lonProp)) {
            return null;
        }

        if (!float.TryParse(latProp.GetString(), CultureInfo.InvariantCulture, out var lat)) {
            return null;
        }

        if (!float.TryParse(lonProp.GetString(), CultureInfo.InvariantCulture, out var lon)) {
            return null;
        }

        return (lat, lon);
    }
}