namespace projekt_io.Models;

public class RolesModalViewModel {
    public string Id { get; set; }
    public string Title { get; set; }
    public string Action { get; set; }
    public string Controller { get; set; }
    public List<string> AvailableRoles { get; set; }
}