namespace projekt_io.Entities;

public class Chat {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? LostReportId { get; set; }
    public LostReport? LostReport { get; set; }
    
    public string? SightingId { get; set; }
    public Sighting? Sighting { get; set; }

    public string CreatorId { get; set; }
    public ApplicationUser Creator { get; set; }
    
    public string OwnerId { get; set; }
    public ApplicationUser Owner { get; set; }

    public List<Message> Messages { get; set; } = new List<Message>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}