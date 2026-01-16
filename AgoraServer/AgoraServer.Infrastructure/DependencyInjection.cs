using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// 
/// this is a dependency injection class. 'AddInfrastructure' is called inside program.cs in order to have
/// access to the /infrastructure project repositories
/// 
/// </summary>

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Get the appropriate connection string based on environment
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var connectionString = Environment.GetEnvironmentVariable(environment == "Development" 
            ? "CONNECTION_STRING_DEVELOPMENT" 
            : "CONNECTION_STRING_PRODUCTION");
        
        configuration["ConnectionStrings:DefaultConnection"] = connectionString;
        
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
        
        // Register scoped repo
        
        return services;
    }
}