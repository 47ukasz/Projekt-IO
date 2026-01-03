using projekt_io.DTOs;

namespace projekt_io.Services;

public interface ILostReportService {
    public Task<bool> CreateAsync(string ownerId, LostReportDto lostReportDto, IFormFile photo);
}