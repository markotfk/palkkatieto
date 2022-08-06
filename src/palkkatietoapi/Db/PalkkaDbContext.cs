using Microsoft.EntityFrameworkCore;
using palkkatietoapi.Model;

namespace palkkatietoapi.Db;

public class PalkkaDbContext : DbContext
{
    public PalkkaDbContext(DbContextOptions<PalkkaDbContext> options) : base(options) 
    {        
    }
    
    public DbSet<User>? Users { get; set; }
    public DbSet<Palkka>? Palkat { get; set; }
}

