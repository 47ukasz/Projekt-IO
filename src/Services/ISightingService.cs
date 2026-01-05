using projekt_io.DTOs;

namespace projekt_io.Services;

public interface ISightingService {
    public Task<bool> CreateAsync(string userId, SightingDto sightingDto, IFormFile photo);
}