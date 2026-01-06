namespace projekt_io.DTOs;

public class SightingDto { 
    public string? Id { get; set; }
    public string LostReportId { get; set; }
    public string Description { get; set; }
    public LocationDto Location { get; set; }
    public LostReportDto? LostReport { get; set; }
    public DateOnly SeenDate { get; set; }
    public TimeOnly SeenTime { get; set; } 
}