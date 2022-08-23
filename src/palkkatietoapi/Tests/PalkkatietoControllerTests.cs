namespace palkkatietoapi.Tests;

using palkkatietoapi.Controllers;
using palkkatietoapi.Model;
using NUnit.Framework;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PalkkatietoControllerTests : UnitTestBase
{
    private PalkkatietoController instance; 
    private const string userLogin = nameof(PalkkatietoControllerTests);
    private const string userName = "Test PalkkatietoController";

    protected override void Setup() {
        var mockLogger = Substitute.For<ILogger<PalkkatietoController>>();
        IPalkkatietoService unitTestPalkkaService = new PalkkatietoService(db);
        instance = new PalkkatietoController(mockLogger, unitTestPalkkaService);
    }

    [Test]
    public async Task AddPalkkaTest() 
    {
        var palkka = CreatePalkka(5654, "Barcelona", "Meta", "Boss");
        var result = await instance.AddPalkka(palkka, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(result);
        var palkkaResult = result.Value as Palkka;
        Assert.NotNull(palkkaResult);
        Assert.IsTrue(palkkaResult.Id != 0);
    }

    [Test]
    public async Task GetByIdTest() 
    {
        var palkka = CreatePalkka(9999, "New York", "Facebook", "Test engineer");
        var result = (OkObjectResult)await instance.AddPalkka(palkka, CancellationToken.None);
        var addPalkkaResult = (Palkka)result.Value;
        Assert.IsTrue(addPalkkaResult.Id != 0);
        var getResult = await instance.GetById(addPalkkaResult.Id) as OkObjectResult;
        Assert.NotNull(getResult);
        var getResultPalkka = (Palkka)getResult.Value;
        Assert.AreEqual(addPalkkaResult.Id, getResultPalkka.Id);
        Assert.AreEqual(getResultPalkka.Amount, 9999);
        Assert.AreEqual(getResultPalkka.City, "New York");
        Assert.AreEqual(getResultPalkka.Company, "Facebook");
        Assert.AreEqual(getResultPalkka.CountryCode, "fi");
        Assert.AreEqual(getResultPalkka.JobRole, "Test engineer");
        Assert.NotNull(getResultPalkka.User);
    }
    
    [Test]
    public async Task GetByQueryMinAmountTest() 
    {
        var palkka = CreatePalkka(10000, "Vaasa", "CompanyA", "worker");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = CreatePalkka(2000, "Kemi", "CompanyB", "roleX");
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
        var palkka = CreatePalkka(3000, "Oulu", "Oura", "Coder");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = CreatePalkka(6000, "Helsinki", "HSL", "Driver");
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
        var palkka = CreatePalkka(3020, "Muhos", "IBM", "Analyst");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = CreatePalkka(5000, "Rovaniemi", "Joulupukin paja", "Joulupukki");
        await instance.AddPalkka(palkka2, CancellationToken.None);
        var palkka3 = CreatePalkka(9000, "Lahti", "Scandic", "secretary");
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
        var palkka = CreatePalkka(6020, "Birginham", "Server Inc.", "admin");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = CreatePalkka(5000, "Auckland", "Google", "evil boss");
        await instance.AddPalkka(palkka2, CancellationToken.None);
        var palkka3 = CreatePalkka(2000, "Copenhagen", "R-collection", "dresser");
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
        var palkka = CreatePalkka(6020, "Birginham", "Server Inc.", "admin");
        await instance.AddPalkka(palkka, CancellationToken.None);
        var palkka2 = CreatePalkka(5000, "Auckland", "Google", "AI engineer");
        await instance.AddPalkka(palkka2, CancellationToken.None);
        var palkka3 = CreatePalkka(2000, "Copenhagen", "R-collection", "Foo");
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

    [Test]
    public void TestAddPalkkaCompanyNull()
    {
        var palkka = CreatePalkka(222,"city",null,"jobrole");

        Assert.ThrowsAsync<DbUpdateException>(async () => await instance.AddPalkka(palkka, CancellationToken.None), "Add with null company should throw DbUpdateException");
    }

    [Test]
    public void TestAddPalkkaCityNull()
    {
        var palkka = CreatePalkka(232, null, "company", "jobrole");
        Assert.ThrowsAsync<DbUpdateException>(async () => await instance.AddPalkka(palkka, CancellationToken.None), "Add with null city should throw DbUpdateException");
    }

    [Test]
    public async Task TestGetByIdNotFound()
    {
        var palkka = CreatePalkka(2000, "city", "company", "role");
        await instance.AddPalkka(palkka, CancellationToken.None);

        var getByIdResult = await instance.GetById(-1) as NotFoundResult;
        Assert.IsNotNull(getByIdResult);
    }
    
}
