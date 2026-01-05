namespace projekt_io.Entities;

public class Sighting {
    public string Id {get; set;} = Guid.NewGuid().ToString();
    
    public string UserId {get; set;}
    public ApplicationUser? User {get; set;}
    
    public string LostReportId {get; set;}
    public LostReport? LostReport {get; set;}
    
    public string LocationId {get; set;}
    public Location? Location {get; set;}
    
    public string Description {get; set;}
    public string PhotoPath {get; set;}
    public DateOnly SeenDate { get; set; }
    public DateOnly CreatedAt {get; set;} = DateOnly.FromDateTime(DateTime.UtcNow);
}