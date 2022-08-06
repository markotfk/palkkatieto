
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
    public void Setup() {
        var mockLogger = Substitute.For<ILogger<PalkkatietoController>>();
        db = UnitTestPalkkaDbContextCreator.Instance();
        db.Database.Migrate();
        
        db.Users.Add(GetUser());
        db.SaveChanges();
        var unitTestPalkkaService = new PalkkatietoService(db);
        instance = new PalkkatietoController(mockLogger, unitTestPalkkaService);
    }

    [Test]
    public async Task AddPalkka() 
    {
        var palkka = new Palkka();
        palkka.Amount = 10000;
        palkka.City = "Oulu";
        palkka.Company = "Nokia";
        palkka.CountryCode = "fi";
        palkka.DateReported = DateTime.Today.ToUniversalTime();
        palkka.JobRole = "Boss";
        palkka.User = db.Users.First(u => u.Login == "unit_test_login");
        await instance.AddPalkka(palkka, CancellationToken.None);
    }
    
    [Test]
    public async Task TestPalkkatietoController_GetByQuery() 
    {
        var t = await instance.GetByQuery(new PalkkaQuery(), CancellationToken.None);
        var jsonResult = t as JsonResult;
        Assert.NotNull(jsonResult);
        Assert.AreEqual(200, jsonResult.StatusCode);
    }

    private User GetUser() {
        var user = new User();
        user.Login = "unit_test_login";
        user.Name = "Unit Test";
        user.LastLogin = DateTime.UtcNow;
        return user;
    }
}
