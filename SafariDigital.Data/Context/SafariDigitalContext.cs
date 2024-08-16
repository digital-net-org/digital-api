using Microsoft.EntityFrameworkCore;
using SafariDigital.Data.Models.Database;

namespace SafariDigital.Data.Context;

public class SafariDigitalContext(DbContextOptions<SafariDigitalContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Avatar> Avatars { get; set; }
    public DbSet<RecordedLogin> RecordedLogins { get; set; }
    public DbSet<RecordedToken> RecordedTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Avatar>().Navigation(s => s.Document).AutoInclude();
        modelBuilder.Entity<User>().Navigation(s => s.Avatar).AutoInclude();
    }
}