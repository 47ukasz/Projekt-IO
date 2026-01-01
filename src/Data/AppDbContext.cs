using Microsoft.EntityFrameworkCore;

namespace projekt_io.Data;

public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}