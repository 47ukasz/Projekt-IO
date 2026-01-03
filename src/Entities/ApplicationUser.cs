using Microsoft.AspNetCore.Identity;

namespace projekt_io.Entities;

public class ApplicationUser : IdentityUser {
    public bool EnableNotifications { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Blocked { get; set; }
    
    public List<Animal> Animals { get; set; }
    public List<LostReport> LostReports { get; set; }
}