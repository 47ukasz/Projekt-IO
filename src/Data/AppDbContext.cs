using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using projekt_io.Entities;

namespace projekt_io.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>{
    public DbSet<LostReport> LostReports { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Location> Locations { get; set; } 
    public DbSet<Sighting> Sightings { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity => {
            entity.Property(e => e.EnableNotifications).HasDefaultValue(true);
            entity.Property(e => e.Blocked).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasDefaultValue("");
        });
        
        modelBuilder.Entity<Sighting>()
            .Property(x => x.SeenDate)
            .HasColumnType("timestamp without time zone");
    }
}