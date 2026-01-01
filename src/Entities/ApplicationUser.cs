using Microsoft.AspNetCore.Identity;

namespace projekt_io.Entities;

public class ApplicationUser : IdentityUser {
    public bool EnableNotifications { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}