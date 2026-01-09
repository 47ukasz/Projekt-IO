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
    public DbSet<ChatParticipant> ChatParticipants { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity => {
            entity.Property(e => e.EnableNotifications).HasDefaultValue(true);
            entity.Property(e => e.Blocked).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasDefaultValue("");
        });
        
        modelBuilder.Entity<ChatParticipant>(entity => {
            entity.HasKey(x => new { x.ChatId, x.UserId });

            entity.HasOne(x => x.Chat)
                .WithMany(c => c.Participants)
                .HasForeignKey(x => x.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Message>(entity => {
            entity.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(m => m.Content).HasMaxLength(2000);
        });
        
        modelBuilder.Entity<Sighting>()
            .Property(x => x.SeenDate)
            .HasColumnType("timestamp without time zone");
    }
}