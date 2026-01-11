using projekt_io.DTOs;

namespace projekt_io.Models;

public class ReportsViewModel {
    public List<LostReportDto> LostReports { get; set; }
    public PaginationInfoModel PaginationInfo { get; set; }
}