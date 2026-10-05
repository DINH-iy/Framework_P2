using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Game.Domain;
using Game.Api.Services;
using Game.Api.Middleware;

namespace Game.Features;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        Configure(app);

        app.Run();
    }

    private static void Configure(WebApplication app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.MapControllers();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured.");

        builder.Services.AddDbContext<GameDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

        builder.Services.AddScoped<GameService>();
        builder.Services.AddScoped<CombatService>();
        builder.Services.AddScoped<CombatRoundService>();
        builder.Services.AddScoped<GameStateService>();
        builder.Services.AddScoped<GameResponseMapper>();
        builder.Services.AddScoped<GameStartService>();
        builder.Services.AddScoped<GameQueryService>();
        builder.Services.AddScoped<MovementService>();
        builder.Services.AddScoped<CombatGameService>();
        builder.Services.AddScoped<CombatRoundService>();
        
        builder.Services.AddControllers().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }
}
