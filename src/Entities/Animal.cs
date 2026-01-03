namespace projekt_io.Entities;

public class Animal {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string Name { get; set; }
    public string Species { get; set; }
    public string Breed { get; set; }
    public string Description { get; set; }
    public string PhotoPath { get; set; }
}