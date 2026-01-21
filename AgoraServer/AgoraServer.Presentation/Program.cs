using Agora.Core.Settings;
using Application;
using Domain.Interfaces.Proxies;
using Infrastructure;
using Presentation;
using Presentation.HubController;
using Presentation.HubProxy;

// NOTE : for jwt token, follow https://www.youtube.com/watch?v=w8I32UPEvj8

// load environment variables
DotNetEnv.Env.Load();

// build
var builder = WebApplication.CreateBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = null;
});

// Allow specific origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("DiscordPolicy",
        policy =>
        {
            policy.SetIsOriginAllowed(origin => 
                {
                    if (string.IsNullOrEmpty(origin)) return true;
                    if (origin == "null") return true;
                    
                    if (origin.Contains("localhost")) return true;
                    if (origin.Contains(".amplifyapp.com")) return true;
                    if (origin.Contains(".discordsays.com")) return true;
                    if (origin == "https://discordsays.com") return true;
                    
                    Console.WriteLine($"[CORS] Rejected Origin: {origin}");

                    return false;
                })
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials(); // <--- CRITICAL for SignalR
        });
});

// add authentication
// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(options =>
// {
//     options.RequireHttpsMetadata = false; // should set to true in production
//     options.SaveToken = true;
//     options.TokenValidationParameters = new TokenValidationParameters()
//     {
//         ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
//         ValidAudience = builder.Configuration["JwtConfig:Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//     };
// });
//
// builder.Services.AddAuthorization();


// register services
builder.Services
    .AddSingleton<Startup>()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddAuthorization()
    .AddControllers();

// add health checks support
builder.Services.AddHealthChecks();

// add Http support
builder.Services.AddHttpClient();

// Add SignalR support
builder.Services
    .AddSignalR(options =>
    {
        options.EnableDetailedErrors = true;
        options.KeepAliveInterval = TimeSpan.FromMinutes(20);
        ;
        options.HandshakeTimeout = TimeSpan.FromMinutes(20);
        ;
    });
    // .AddJsonProtocol(options =>
    // {
    //     options.PayloadSerializerOptions.Converters.Add(new PolymorphicConverterFactory());
    // });

// Register HubServices
builder.Services.AddSingleton<HubProxy>();
builder.Services.AddSingleton<IGameProxy>(sp => sp.GetRequiredService<HubProxy>());
builder.Services.AddSingleton<ILobbyProxy>(sp => sp.GetRequiredService<HubProxy>());

builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();

var app = builder.Build();

// Make sure CORS is enabled before SignalR
app.UseCors("DiscordPolicy");

// Configure the Http request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllers();

app.MapHealthChecks("/health");

app.MapHub<HubController>("/hub");

// run startup
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var startup = app.Services.GetRequiredService<Startup>();

lifetime.ApplicationStarted.Register(() =>
{
    startup.OnStartedAsync();
});

app.Run();