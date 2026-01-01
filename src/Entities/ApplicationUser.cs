using Microsoft.AspNetCore.Identity;

namespace projekt_io.Entities;

public class ApplicationUser : IdentityUser {
    public bool EnableNotifications { get; set; }
}