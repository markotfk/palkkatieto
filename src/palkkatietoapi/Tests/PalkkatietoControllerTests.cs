
namespace palkkatietoapi.Tests;

using palkkatietoapi.Controllers;
using palkkatietoapi.Model;
using palkkatietoapi.Db;
using NUnit.Framework;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
public class PalkkatietoControllerTests {

    private PalkkatietoController instance;
    private PalkkaDbContext db;

    [SetUp]
    public async Task Setup() {
        var mockLogger = Substitute.For<ILogger<PalkkatietoController>>();
        db = UnitTestPalkkaDbContextCreator.Instance();
        await db.Database.MigrateAsync();

        await db.Database.ExecuteSqlRawAsync("DELETE FROM \"Palkat\"");
        await db.Database.ExecuteSqlRawAsync("DELETE FROM \"Users\"");
        
        await db.Users.AddAsync(GetUser());
        await db.SaveChangesAsync();
        var unitTestPalkkaService = new PalkkatietoService(db);
        instance = new PalkkatietoController(mockLogger, unitTestPalkkaService);
    }

    [TearDown]
    public void Teardown() 
    {
        db?.Dispose();
    }

    [Test]
    public async Task AddPalkkaTest() 
    {
        var palkka = GetPalkka(5654, "Barcelona", "Meta");
        var result = await instance.AddPalkka(palkka, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(result);
        var palkkaResult = result.Value as Palkka;
        Assert.NotNull(palkkaResult);
        Assert.IsTrue(palkkaResult.Id != 0);
    }

    [Test]
    public async Task GetByIdTest() 
    {
        var palkka = GetPalkka(9999, "New York", "Facebook");
        var result = (OkObjectResult)await instance.AddPalkka(palkka, CancellationToken.None);
        var addPalkkaResult = (Palkka)result.Value;
        var getResult = await instance.GetById(addPalkkaResult.Id) as OkObjectResult;
        Assert.NotNull(getResult);
        var getResultPalkka = (Palkka)getResult.Value;
        Assert.AreEqual(addPalkkaResult.Id, getResultPalkka.Id);
        Assert.AreEqual(getResultPalkka.Amount, 9999);
        Assert.AreEqual(getResultPalkka.City, "New York");
        Assert.AreEqual(getResultPalkka.Company, "Facebook");
        Assert.AreEqual(getResultPalkka.CountryCode, "fi");
        Assert.AreEqual(getResultPalkka.JobRole, "Boss");
        Assert.NotNull(getResultPalkka.User);
    }
    
    [Test]
    public async Task GetByQueryMinAmountTest() 
    {
        var palkka = GetPalkka(10000, "Vaasa", "CompanyA");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = GetPalkka(2000, "Kemi", "CompanyB");
        await instance.AddPalkka(palkka2, CancellationToken.None);
        
        var palkkaQuery = new PalkkaQuery() {
            AmountMin = 10000
        };
        var t = await instance.GetByQuery(palkkaQuery, CancellationToken.None);
        var result = t as OkObjectResult;
        Assert.NotNull(result);

        var resultValue = (List<Palkka>)result.Value;
        Assert.AreEqual(1, resultValue.Count);
        Assert.AreEqual(10000, resultValue[0].Amount);
        Assert.AreEqual("Vaasa", resultValue[0].City);
        Assert.AreEqual("CompanyA", resultValue[0].Company);
    }

    [Test]
    public async Task GetByQueryCityTest() 
    {
        var palkka = GetPalkka(3000, "Oulu", "Oura");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = GetPalkka(6000, "Helsinki", "HSL");
        await instance.AddPalkka(palkka2, CancellationToken.None);
        
        var palkkaQuery = new PalkkaQuery() {
            City = "Oulu"
        };
        var t = await instance.GetByQuery(palkkaQuery, CancellationToken.None);
        var result = t as OkObjectResult;
        Assert.NotNull(result);

        var resultValue = (List<Palkka>)result.Value;
        Assert.AreEqual(1, resultValue.Count);
        Assert.AreEqual("Oulu", resultValue[0].City);
        Assert.AreEqual("Oura", resultValue[0].Company);
    }

    [Test]
    public async Task GetByQueryTest() 
    {
        var palkka = GetPalkka(3020, "Muhos", "IBM");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = GetPalkka(5000, "Rovaniemi", "Joulupukin paja");
        await instance.AddPalkka(palkka2, CancellationToken.None);
        var palkka3 = GetPalkka(9000, "Lahti", "Scandic");
        await instance.AddPalkka(palkka3, CancellationToken.None);
        
        // No query parameters, should return all items in random order
        var palkkaQuery = new PalkkaQuery();
        var t = await instance.GetByQuery(palkkaQuery, CancellationToken.None);
        var result = t as OkObjectResult;
        Assert.NotNull(result);

        var resultValue = (List<Palkka>)result.Value;
        Assert.AreEqual(3, resultValue.Count);
    }

    [Test]
    public async Task GetByQueryOrderByTest() 
    {
        var palkka = GetPalkka(6020, "Birginham", "Server Inc.");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = GetPalkka(5000, "Auckland", "Google");
        await instance.AddPalkka(palkka2, CancellationToken.None);
        var palkka3 = GetPalkka(2000, "Copenhagen", "R-collection");
        await instance.AddPalkka(palkka3, CancellationToken.None);
        
        // Order results by city
        var palkkaQuery = new PalkkaQuery {
            OrderBy = nameof(Palkka.City)
        };
        var t = await instance.GetByQuery(palkkaQuery, CancellationToken.None);
        var result = t as OkObjectResult;
        Assert.NotNull(result);

        var resultValue = (List<Palkka>)result.Value;
        Assert.AreEqual(3, resultValue.Count);
        // First item should be Auckland
        Assert.AreEqual("Auckland", resultValue[0].City);
        Assert.AreEqual(5000, resultValue[0].Amount);
        Assert.AreEqual("Google", resultValue[0].Company);
    }

    [Test]
    public async Task GetByQueryOrderByDescendingTest() 
    {
        var palkka = GetPalkka(6020, "Birginham", "Server Inc.");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = GetPalkka(5000, "Auckland", "Google");
        await instance.AddPalkka(palkka2, CancellationToken.None);
        var palkka3 = GetPalkka(2000, "Copenhagen", "R-collection");
        await instance.AddPalkka(palkka3, CancellationToken.None);

        // Test descending ordering
        var palkkaQuery = new PalkkaQuery {
            OrderBy = nameof(Palkka.City)
        };
        palkkaQuery.OrderByDescending = true;
        var orderByDescResult = await instance.GetByQuery(palkkaQuery, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(orderByDescResult);
        
        var resultValue = (List<Palkka>)orderByDescResult.Value;
        Assert.AreEqual(3, resultValue.Count);
        // Descending order should return Copenhagen first
        Assert.AreEqual("Copenhagen", resultValue[0].City);
        Assert.AreEqual(2000, resultValue[0].Amount);
        Assert.AreEqual("R-collection", resultValue[0].Company);
    }

    private User GetUser() {
        var user = new User();
        user.Login = "unit_test_login";
        user.Name = "Unit Test";
        user.LastLogin = DateTime.UtcNow;
        return user;
    }

    private Palkka GetPalkka(decimal amount, string city, string company) {
        var palkka = new Palkka();
        palkka.Amount = amount;
        palkka.City = city;
        palkka.Company = company;
        palkka.CountryCode = "fi";
        palkka.DateReported = DateTime.Today.ToUniversalTime();
        palkka.JobRole = "Boss";
        palkka.User = db.Users.First(u => u.Login == "unit_test_login");
        return palkka;
    }
}
