using projekt_io.DTOs;
using projekt_io.Entities;

namespace projekt_io.Services;

public interface ILostReportService {
    public Task<bool> CreateAsync(string ownerId, LostReportDto lostReportDto, IFormFile photo);
    public Task<bool> UpdateAsync(string ownerId, LostReportDto lostReportDto, IFormFile photo);
    public Task<List<LostReportDto>> GetLostReportsByIdAsync(string ownerId);
    public Task<List<LostReportDto>> GetAllLostReportsAsync();
    public Task<LostReportDto> GetLostReportByIdAsync(string ownerId, string reportId);
}