
namespace palkkatietoapi.Tests;

using Microsoft.EntityFrameworkCore;
using palkkatietoapi.Db;

public static class UnitTestPalkkaDbContextCreator {

    public static PalkkaDbContext Instance() {
        var connectionString = "Server=localhost:5432;Username=palkka_test;Password=dFe65jHf3!556;Database=palkkatieto_unit_test;";
        var optionsBuilder = new DbContextOptionsBuilder<PalkkaDbContext>().UseNpgsql(connectionString).EnableDetailedErrors(true);
        
        return new PalkkaDbContext(optionsBuilder.Options);
    }

}