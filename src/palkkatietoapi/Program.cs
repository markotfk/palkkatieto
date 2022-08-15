using palkkatietoapi.Db;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace palkkatietoapi;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs/palkkatietoapi.txt", rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        
        var builder = WebApplication.CreateBuilder(args);
        
        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);

        var env = builder.Environment.EnvironmentName;

        Console.WriteLine($"Palkkatietoapi starting in {env} environment.");

        var app = builder.Build();
        if (env == Environments.Development || env == Environments.Staging)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<PalkkaDbContext>();
                await db.Database.MigrateAsync();
            }

            await startup.ConfigureAndRun(app, builder.Environment);
        }
    }
}