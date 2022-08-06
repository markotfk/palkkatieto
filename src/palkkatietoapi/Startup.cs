using Microsoft.EntityFrameworkCore;

namespace palkkatietoapi;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<palkkatietoapi.Db.PalkkaDbContext>(options => {
            options.UseNpgsql(Configuration.GetConnectionString("PalkkaDb"));
        });
    }

    public void ConfigureAndRun(WebApplication app, IWebHostEnvironment env)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
            
        } else {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        
        app.Run();
    }
}