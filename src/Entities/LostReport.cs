namespace projekt_io.Entities;

public class LostReport {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string UserId { get; set; }
    public ApplicationUser? User { get; set; }
    
    public string AnimalId { get; set; }
    public Animal? Animal { get; set; }
    
    public string LocationId { get; set; }
    public Location? Location { get; set; }
    
    public string Title { get; set; }
    public string Status { get; set; }
    
    public DateOnly CreatedAt { get; set; }
    public DateOnly UpdatedAt { get; set; }
    public DateOnly LostAt { get; set; }
}