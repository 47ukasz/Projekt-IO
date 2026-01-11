using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using projekt_io.Entities;

namespace projekt_io.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>{
    public DbSet<LostReport> LostReports { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Location> Locations { get; set; } 
    public DbSet<Sighting> Sightings { get; set; }
    
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.Creator)
            .WithMany()
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.Owner)
            .WithMany()
            .HasForeignKey(c => c.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}