using projekt_io.Data;
using projekt_io.DTOs;
using projekt_io.Entities;
using projekt_io.Mappers;

namespace projekt_io.Services;

public class SightingService : ISightingService{
    private readonly ILogger<SightingService> _logger;
    private readonly IGeocodingService _geocodingService;
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public SightingService(ILogger<SightingService> logger, IGeocodingService geocodingService, AppDbContext db, IWebHostEnvironment env) {
        _logger = logger;
        _geocodingService = geocodingService;
        _db = db;
        _env = env;
    }

    public async Task<bool> CreateAsync(string userId, SightingDto sightingDto, IFormFile photo) {
        Console.WriteLine("done 1");
        
        if (string.IsNullOrWhiteSpace(userId)) {
            return false;
        }
        
        Console.WriteLine("done 2");

        if (sightingDto == null) {
            return false;
        }

        Console.WriteLine("done 3");
        
        if (sightingDto.Location == null) {
            return false;
        }
        
        Console.WriteLine("done 4");
        
        await using var dbTransaction = await _db.Database.BeginTransactionAsync();

        try {
            var location = await CreateLocationAsync(sightingDto.Location);

            if (location == null) {
                throw new Exception("Failed at saving...");
            }
            
            var sightingEntity = SightingMapper.ToEntity(sightingDto);
            
            var photoPath = await SavePhotoAsync(photo);
            
            sightingEntity.UserId = userId;
            sightingEntity.LostReportId = sightingDto.LostReportId;
            sightingEntity.LocationId = location.Id;
            sightingEntity.PhotoPath = photoPath;

            sightingEntity.Location = location;
            
            _db.Sightings.Add(sightingEntity);
            
            var count = await _db.SaveChangesAsync();
            
            if (count <= 0) {
                throw new Exception("Failed at saving...");
            }
            
            await dbTransaction.CommitAsync();

            return true;
        } catch (Exception ex) {
            _logger.LogError($"Failed to save sighting: {ex.Message}");
            _logger.LogError(ex,
                "CreateAsync failed. userId={UserId}, lostReportId={LostReportId}, lat={Lat}, lng={Lng}, seenDate={SeenDate}",
                userId, sightingDto?.LostReportId, sightingDto?.Location?.Latitude, sightingDto?.Location?.Longitude, sightingDto?.SeenDate);
            await dbTransaction.RollbackAsync();
            return false;
        }
    }
    
    private async Task<Location?> CreateLocationAsync(LocationDto locationDto) {
        var location = LocationMapper.ToEntity(locationDto);
        var city = await _geocodingService.GetCityAsync(location.Latitude, location.Longitude);
        location.City = city ?? "";
        
        _db.Locations.Add(location);
 
        var count = await _db.SaveChangesAsync();

        if (count > 0) { 
            return location;
        }
        
        return null;
    }
    
    private async Task<string> SavePhotoAsync(IFormFile photo) {
        var defaultPath = "/uploads/default-animal.png";

        if (photo == null || photo.Length == 0) {
            return defaultPath;
        }
        
        var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsDir)) {
            Directory.CreateDirectory(uploadsDir);
        }
        
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
        
        var path = Path.Combine(uploadsDir, fileName);
        
        using var stream = new FileStream(path, FileMode.Create);
        await photo.CopyToAsync(stream);
        
        return $"/uploads/{fileName}";
    }
}