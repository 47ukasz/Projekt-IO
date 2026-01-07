using Microsoft.EntityFrameworkCore;
using projekt_io.Data;
using projekt_io.DTOs;
using projekt_io.Entities;
using projekt_io.Mappers;

namespace projekt_io.Services;

public class LostReportService : ILostReportService {
    private readonly ILogger<LostReportService> _logger;
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly IGeocodingService _geocodingService;

    public LostReportService(ILogger<LostReportService> logger, AppDbContext db, IWebHostEnvironment env, IGeocodingService geocodingService) {
        _logger = logger;
        _db = db;
        _env = env;
        _geocodingService = geocodingService;
    }

    public async Task<bool> CreateAsync(string ownerId, LostReportDto lostReportDto, IFormFile photo) {
        if (string.IsNullOrWhiteSpace(ownerId)) {
            return false;
        }

        if (lostReportDto == null) {
            return false;
        }

        if (lostReportDto.Animal == null || lostReportDto.Location == null) {
            return false;
        }

        await using var dbTransaction = await _db.Database.BeginTransactionAsync();

        try {
            var location = await CreateLocationAsync(lostReportDto.Location);

            if (location == null) {
                throw new Exception("Failed at saving...");
            }

            var animal = await CreateAnimalAsync(ownerId, lostReportDto.Animal, photo);

            if (animal == null) {
                throw new Exception("Failed at saving...");
            }

            var reportEntity = LostReportMapper.ToEntity(lostReportDto);

            reportEntity.UserId = ownerId;
            reportEntity.LocationId = location.Id;
            reportEntity.AnimalId = animal.Id;

            reportEntity.Animal = null;
            reportEntity.Location = null;
            
            _db.LostReports.Add(reportEntity);

            var count = await _db.SaveChangesAsync();
            
            if (count <= 0) {
                throw new Exception("Failed at saving...");
            }
            
            await dbTransaction.CommitAsync();

            return true;
        } catch (Exception ex) {
            _logger.LogError($"Failed to save sighting: {ex.Message}");
            await dbTransaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> UpdateAsync(string ownerId, LostReportDto lostReportDto, IFormFile photo) {
        var reportId = lostReportDto.Id;
        var reportToUpdate = await _db.LostReports.Include(r => r.Animal).Include(r => r.Location)
            .FirstOrDefaultAsync(r => r.UserId == ownerId && r.Id == reportId);

        if (reportToUpdate == null) {
            return false;
        }
        
        reportToUpdate.Title = lostReportDto.Title.ToLower();
        reportToUpdate.Status = lostReportDto.Status.ToLower();
        reportToUpdate.UpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        reportToUpdate.LostAt = lostReportDto.LostAt ?? DateOnly.FromDateTime(DateTime.UtcNow);
        
        reportToUpdate.Animal.Name = lostReportDto.Animal.Name.ToLower();
        reportToUpdate.Animal.Species = lostReportDto.Animal.Species.ToLower();
        reportToUpdate.Animal.Breed = lostReportDto.Animal.Breed.ToLower();
        reportToUpdate.Animal.Description = lostReportDto.Animal.Description.ToLower();
        
        reportToUpdate.Location.Latitude = lostReportDto.Location.Latitude;
        reportToUpdate.Location.Longitude = lostReportDto.Location.Longitude;
        reportToUpdate.Location.City = await _geocodingService.GetCityAsync(lostReportDto.Location.Latitude, lostReportDto.Location.Longitude);

        if (photo != null && photo.Length > 0) {
            var photoPath = await SaveAnimalPhotoAsync(photo);
            
            reportToUpdate.Animal.PhotoPath = photoPath;
        }
        
        var count = await _db.SaveChangesAsync();

        if (count > 0) {
            return true;
        }

        return false;
    }
    
    public async Task<bool> ChangeStatusAsync(string reportId, string statusValue) {
        var reportToUpdate = await _db.LostReports.FirstOrDefaultAsync(r => r.Id == reportId);

        if (reportToUpdate == null) {
            return false;
        }

        reportToUpdate.Status = statusValue;
        
        var count = await _db.SaveChangesAsync();

        if (count > 0) {
            return true;
        }

        return false;
    }

    public async Task<List<LostReportDto>> GetLostReportsByIdAsync(string ownerId) {
        if (string.IsNullOrWhiteSpace(ownerId)) {
            return null;
        }
        
        var reports = await _db.LostReports.AsNoTracking().Include(r => r.Animal).Include(r => r.Location).Where(r => r.UserId == ownerId).ToListAsync();
        var reportDtos = reports.Select(r => LostReportMapper.ToDto(r)).ToList();
        
        return reportDtos;
    }

    public async Task<List<LostReportDto>> GetAllLostReportsAsync() {
        var reports = await _db.LostReports.AsNoTracking().Include(r => r.Animal).Include(r => r.Location).ToListAsync();
        
        var reportDtos = reports.Select(r => LostReportMapper.ToDto(r)).ToList();
        return reportDtos;
    }

    public async Task<LostReportDto> GetLostReportByIdAsync(string reportId) {
        if (string.IsNullOrWhiteSpace(reportId)) {
            return null;
        }
        
        var report = await _db.LostReports.Include(r => r.Animal).Include(r => r.Location).FirstOrDefaultAsync(report => report.Id == reportId);

        if (report == null) {
            return null;
        }
        
        var reportDto = LostReportMapper.ToDto(report);
        
        return reportDto;
    }

    private async Task<Animal?> CreateAnimalAsync(string ownerId, AnimalDto animalDto, IFormFile photo) {
        var animal = AnimalMapper.ToEntity(animalDto);
        var photoPath = await SaveAnimalPhotoAsync(photo);
        
        animal.UserId = ownerId;
        animal.Id = Guid.NewGuid().ToString();
        animal.PhotoPath = photoPath;
        
        _db.Animals.Add(animal);

        var count = await _db.SaveChangesAsync();

        if (count > 0) {
            return animal;
        }
        
        return null;
    }

    public async Task<bool> DeleteAsync(string reportId) {
        if (string.IsNullOrWhiteSpace(reportId)) {
            return false;
        }
        
        var reportToDelete = await _db.LostReports.Include(r => r.Animal).Include(r => r.Location).FirstOrDefaultAsync(r => r.Id == reportId);

        if (reportToDelete == null) {
            return false;
        }
        
        var sightings = await _db.Sightings.Where(s => s.LostReportId == reportId).ToListAsync();

        if (sightings.Count > 0) {
            _db.Sightings.RemoveRange(sightings);
        }
        
        var animal = reportToDelete.Animal;
        var location = reportToDelete.Location;
        
        _db.LostReports.Remove(reportToDelete);

        if (animal != null) {
            _db.Animals.Remove(animal);
        }

        if (location != null) {
            _db.Locations.Remove(location);
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

    private async Task<string> SaveAnimalPhotoAsync(IFormFile photo) {
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
