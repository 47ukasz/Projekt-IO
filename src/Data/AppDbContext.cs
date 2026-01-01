using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using projekt_io.Entities;

namespace projekt_io.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity => {
            entity.Property(e => e.EnableNotifications).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasDefaultValue("");
        });
    }
}