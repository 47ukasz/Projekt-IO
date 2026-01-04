namespace projekt_io.Entities;

public class Location {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public float Radius { get; set; }
    public string? City { get; set; }
}