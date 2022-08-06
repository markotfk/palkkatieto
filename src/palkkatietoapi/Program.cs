using palkkatietoapi.Db;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var startup = new palkkatietoapi.Startup(builder.Configuration);
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

            startup.ConfigureAndRun(app, builder.Environment);
        }
    }
}