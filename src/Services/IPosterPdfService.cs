using projekt_io.DTOs;

namespace projekt_io.Services;

public interface IPosterPdfService
{
    byte[] GenerateLostReportPoster(LostReportDto report, string? ownerEmail, string baseUrl);
}
