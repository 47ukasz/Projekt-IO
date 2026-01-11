using projekt_io.DTOs;
using projekt_io.Entities;

namespace projekt_io.Services;

public interface ILostReportService {
    public Task<bool> CreateAsync(string ownerId, LostReportDto lostReportDto, IFormFile photo);
    public Task<bool> UpdateAsync(string ownerId, LostReportDto lostReportDto, IFormFile photo);
    public Task<bool> DeleteAsync(string reportId);
    public Task<List<LostReportDto>> GetLostReportsByIdAsync(string ownerId);

    public Task<(List<LostReportDto> Items, int TotalCount)?> GetLostReportsByIdAsync(string ownerId, int page, int pageSize);
    public Task<List<LostReportDto>> GetAllLostReportsAsync();
    public Task<LostReportDto> GetLostReportByIdAsync(string reportId);
    public Task<bool> ChangeStatusAsync(string reportId, string statusValue);

}