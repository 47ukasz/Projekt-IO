namespace projekt_io.DTOs;

public class LostReportDto {
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Status { get; set; }
    
    public AnimalDto Animal { get; set; }
    public LocationDto Location { get; set; }
    
    public DateOnly CreatedAt { get; set; }
    public DateOnly UpdatedAt { get; set; }
    public DateOnly? LostAt { get; set; }
}