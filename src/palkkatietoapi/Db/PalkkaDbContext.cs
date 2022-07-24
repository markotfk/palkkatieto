namespace palkkatietoapi.Db;
using Microsoft.EntityFrameworkCore;
using palkkatietoapi.Model;

public partial class PalkkaDbContext : DbContext
{
    readonly string connectionString;
    public PalkkaDbContext(IConfiguration configuration) 
    {
        this.connectionString = configuration.GetConnectionString("PalkkaDb");
    }

    public DbSet<User>? Users { get; set; }
    public DbSet<Palkka>? Palkat { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
        optionsBuilder.UseNpgsql(connectionString);
    }       
}

