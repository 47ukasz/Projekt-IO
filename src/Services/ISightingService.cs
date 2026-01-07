using projekt_io.DTOs;
using projekt_io.Models;

namespace projekt_io.Services;

public interface ISightingService {
    public Task<bool> CreateAsync(string userId, SightingDto sightingDto, IFormFile photo);
    public Task<bool> UpdateAsync(string userId, SightingDto sightingDto, IFormFile photo);
    public Task<bool> DeleteAsync(string reportId);
    public Task<List<SightingDto>> GetAllSightingsAsync();
    public Task<List<SightingDto>> GetSightingsByIdAsync(string ownerId);
    public Task<SightingDto> GetSightingByIdAsync(string userId, string sightingId);
    public Task<List<SightingsCommentViewModel>> GetSightingsByLostReportIdAsync(string lostReportId);
}