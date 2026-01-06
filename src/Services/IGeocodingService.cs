namespace projekt_io.Services;

public interface IGeocodingService {
    public Task<string?> GetCityAsync(float lat, float lon);
    public Task<(float lat, float lon)?> GeocodeAsync(string cityName);
}