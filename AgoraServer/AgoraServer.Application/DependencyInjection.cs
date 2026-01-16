using Application.Services;
using Domain.Interfaces.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

/// <summary>
/// 
/// this is a dependency injection class. 'AddApplication' is called inside program.cs in order to have
/// access to the /application project commands
/// 
/// </summary>

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddValidatorsFromAssembly(assembly);
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // register services
        services.AddSingleton<IConnectionService, ConnectionService>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IDiscordAuthService, DiscordAuthService>();
        services.AddSingleton<ILobbyService, LobbyService>();
        services.AddSingleton<IGameService, GameService>();

        return services;
    }
}