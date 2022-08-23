using palkkatietoapi.Db;
using palkkatietoapi.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace palkkatietoapi.Tests;

public abstract class UnitTestBase 
{
    protected PalkkaDbContext db;

    [SetUp]
    protected async Task BaseSetup()
    {
        db = UnitTestPalkkaDbContextCreator.Instance();
        await db.Database.MigrateAsync();
        await EmptyTables();
        await db.Users.AddAsync(CreateUser("login", "username"));
        await db.SaveChangesAsync();
        Setup();
    }

    [TearDown]
    protected async Task BaseTeardown() {
        await db.DisposeAsync();
        Teardown();
    }

    protected virtual void Setup() {}

    protected virtual void Teardown() {}

    protected async Task EmptyTables() 
    {
        var txn = await db.Database.BeginTransactionAsync();
        try {
            await db.Database.ExecuteSqlRawAsync("TRUNCATE \"Palkat\", \"Users\" RESTART IDENTITY");
        } catch {
            await txn.RollbackAsync();
            throw;
        }
        await txn.CommitAsync();
    }

    protected User CreateUser(string login, string name) {
        var user = new User();
        user.Login = login;
        user.Name = name;
        user.LastLogin = DateTime.UtcNow;
        return user;
    }

    protected Palkka CreatePalkka(decimal amount, string city, string company, string jobRole) {
        var palkka = new Palkka();
        palkka.Amount = amount;
        palkka.City = city;
        palkka.Company = company;
        palkka.CountryCode = "fi";
        palkka.DateReported = DateTime.Today.ToUniversalTime();
        palkka.JobRole = jobRole;
        palkka.User = db.Users.First();
        return palkka;
    }
}