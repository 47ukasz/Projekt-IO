using Microsoft.EntityFrameworkCore;
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
        if (string.IsNullOrWhiteSpace(userId)) {
            return false;
        }
        if (sightingDto == null) {
            return false;
        }

        if (sightingDto.Location == null) {
            return false;
        }
        
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
            sightingEntity.SeenDate = sightingDto.SeenDate.ToDateTime(sightingDto.SeenTime);

            sightingEntity.Location = location;
            
            _db.Sightings.Add(sightingEntity);
            
            var count = await _db.SaveChangesAsync();
            
            if (count <= 0) {
                throw new Exception("Failed at saving...");
            }
            Console.WriteLine("Done3");
            
            await dbTransaction.CommitAsync();

            return true;
        } catch (Exception ex) {
            _logger.LogError($"Failed to save sighting: {ex.Message}");
            await dbTransaction.RollbackAsync();
            return false;
        }
    }

    public async Task<List<SightingDto>> GetAllSightingsAsync() {
        var sightings = await _db.Sightings.AsNoTracking().Include(s => s.Location).Include(s => s.LostReport).ThenInclude(r => r.Animal).ToListAsync();
        
        var sightingsDto = sightings.Select(s => {
            var dto = SightingMapper.ToDto(s);
            dto.SeenTime = TimeOnly.FromDateTime(s.SeenDate);
            return dto;
        }).ToList();
        
        return sightingsDto;
    }

    public async Task<List<SightingDto>> GetSightingsByIdAsync(string ownerId) {
        if (string.IsNullOrWhiteSpace(ownerId)) {
            return null;
        }
        
        var sightings = await _db.Sightings.AsNoTracking().Include(s => s.Location).Include(s => s.LostReport).ThenInclude(r => r.Animal).Where(s => s.UserId == ownerId).ToListAsync();
        var sightingsDto = sightings.Select(s => {
            var dto = SightingMapper.ToDto(s);
            dto.SeenTime = TimeOnly.FromDateTime(s.SeenDate);
            return dto;
        }).ToList();
        
        return sightingsDto;
    }

    public async Task<SightingDto> GetSightingByIdAsync(string userId, string sightingId) {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(sightingId)) {
            return null;
        }
        
        var sighting = await _db.Sightings.Include(s =>s.Location).Include(s => s.LostReport).FirstOrDefaultAsync(s => s.UserId == userId && s.Id == sightingId);

        if (sighting == null) {
            return null;
        }
        
        var sightingDto = SightingMapper.ToDto(sighting);
        sightingDto.SeenTime = TimeOnly.FromDateTime(sighting.SeenDate);
        
        return sightingDto;
    }

    public async Task<bool> UpdateAsync(string userId, SightingDto sightingDto, IFormFile photo) {
        var sightingId = sightingDto.Id;
        
        var sightingToUpdate = await _db.Sightings.Include(s => s.Location).FirstOrDefaultAsync(s => s.UserId == userId && s.Id == sightingId);

        if (sightingToUpdate == null) {
            return false;
        }
        
        sightingToUpdate.Description = sightingDto.Description;
        sightingToUpdate.SeenDate = sightingDto.SeenDate.ToDateTime(sightingDto.SeenTime);
        
        sightingToUpdate.Location.Latitude = sightingDto.Location.Latitude;
        sightingToUpdate.Location.Longitude = sightingDto.Location.Longitude;
        sightingToUpdate.Location.City = await _geocodingService.GetCityAsync(sightingDto.Location.Latitude, sightingDto.Location.Longitude);

        if (photo != null && photo.Length > 0) {
            var photoPath = await SavePhotoAsync(photo);
            
            sightingToUpdate.PhotoPath = photoPath;
        }
        
        var count = await _db.SaveChangesAsync();

        if (count > 0) {
            return true;
        }

        return false;
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