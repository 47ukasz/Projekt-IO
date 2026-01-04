using projekt_io.DTOs;
using projekt_io.Entities;

namespace projekt_io.Services;

public interface ILostReportService {
    public Task<bool> CreateAsync(string ownerId, LostReportDto lostReportDto, IFormFile photo);
    public Task<List<LostReportDto>> GetAllLostReportsAsync();
}