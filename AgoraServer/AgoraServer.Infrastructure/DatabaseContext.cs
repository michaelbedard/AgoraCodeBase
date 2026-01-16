using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

/// <summary>
/// 
/// this is the class that handles database operations.  We should inject this inside our persistent repository classes
/// 
/// </summary>

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
    
    public override void Dispose()
    {
        Console.WriteLine("DatabaseContext disposed");
        base.Dispose();
    }
    
    // public DbSet<UserData> Users { get; set; }
}