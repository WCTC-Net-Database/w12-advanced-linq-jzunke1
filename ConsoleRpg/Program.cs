using ConsoleRpg.Helpers;
using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleRpg;

public static class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddDbContext<GameContext>(options =>
            options.UseInMemoryDatabase("GameDatabase"));

        services.AddSingleton<OutputManager>();
        services.AddScoped<MenuManager>();
        services.AddScoped<GameEngine>();

        var serviceProvider = services.BuildServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameContext>();
            context.Database.EnsureCreated();
            context.SeedData();
        }

        var gameEngine = serviceProvider.GetRequiredService<GameEngine>();
        gameEngine.Run();
    }
}
