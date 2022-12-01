using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace palkkatietoapi.Db;

public class PalkkaContextFactory : IDesignTimeDbContextFactory<PalkkaDbContext>
{
    public PalkkaDbContext CreateDbContext(string[] args)
    {
        if (args.Length == 0) {
            throw new Exception("Invalid arguments");
        }
        var command = (string)args[0];
        if (command =="ConnectionString") {
            if (!string.IsNullOrWhiteSpace(args[1])) {
                var optionsBuilder = new DbContextOptionsBuilder<PalkkaDbContext>();
                optionsBuilder.UseNpgsql(args[1]);
#if DEBUG 
                optionsBuilder.EnableDetailedErrors(true);
#endif
                return new PalkkaDbContext(optionsBuilder.Options);
            }
            
        }
        throw new Exception("ConnectionString must be provided");
    }
}