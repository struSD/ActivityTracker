using System.Diagnostics;

using ActivityTracker.Contract.Database;

using Microsoft.EntityFrameworkCore;

namespace ActivityTracker.Domain.Database;
public class ActivityTrackerDbContext : DbContext
{
    public DbSet<ActivityUser> ActivityUsers { get; set; }
    public DbSet<User> Users { get; set; }
    public ActivityTrackerDbContext() : base()
    {
    }
    public ActivityTrackerDbContext(DbContextOptions<ActivityTrackerDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ActivityTracker;Username=postgres;Password=261095;");
    }
}

