using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

/// <summary>
/// 
/// Necessary in order to call 'dotnet ef database update'
/// 
/// </summary>

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        // Get the appropriate connection string based on environment
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var connectionString = Environment.GetEnvironmentVariable(environment == "Development" 
            ? "CONNECTION_STRING_DEVELOPMENT" 
            : "CONNECTION_STRING_PRODUCTION");
        
        Console.WriteLine(environment);
        Console.WriteLine(connectionString);
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("Connection string is null or empty");
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.UseMySql(
            connectionString,
            new MySqlServerVersion(new Version(8, 0, 32))
        );

        return new DatabaseContext(optionsBuilder.Options);
    }
}