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

    public LostReportService(ILogger<LostReportService> logger, AppDbContext db, IWebHostEnvironment env) {
        _logger = logger;
        _db = db;
        _env = env;
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
            await dbTransaction.RollbackAsync();
            return false;
        }
    }

    public async Task<List<LostReportDto>> GetAllLostReportsAsync() {
        var reports = await _db.LostReports.AsNoTracking().Include(r => r.Animal).Include(r => r.Location).ToListAsync();
        
        var reportDtos = reports.Select(r => LostReportMapper.ToDto(r)).ToList();
        return reportDtos;
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

    private async Task<Location?> CreateLocationAsync(LocationDto locationDto) {
        var location = LocationMapper.ToEntity(locationDto);

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
