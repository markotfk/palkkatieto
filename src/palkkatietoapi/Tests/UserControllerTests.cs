using Microsoft.AspNetCore.Mvc;
using palkkatietoapi.Controllers;
using palkkatietoapi.Model;
using NUnit.Framework;
using NSubstitute;
using palkkatietoapi.Services;
using Microsoft.EntityFrameworkCore;

namespace palkkatietoapi.Tests;

public class UserControllerTests : UnitTestBase
{
    private UsersController instance;
    private const string userLogin = nameof(PalkkatietoControllerTests);
    private const string userName = "Test UsersController";
    
    protected override void Setup() 
    {
        var mockLoggerService = Substitute.For<ILogger<UserService>>();
        IUserService service = new UserService(mockLoggerService, db);
        var mockLoggerController = Substitute.For<ILogger<UsersController>>();
        instance = new UsersController(mockLoggerController, service);
    }

    [Test]
    public async Task TestUserAdd()
    {
        var user = CreateUser(nameof(UserControllerTests), "Unit Test");
        var lastLoginDate = DateTime.UtcNow;
        user.LastLogin = lastLoginDate;
        var addResult = await instance.Add(user, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(addResult);
        var addedUser = (User)addResult.Value;
        Assert.IsTrue(addedUser.Id != 0);
        Assert.AreEqual(nameof(UserControllerTests), addedUser.Login);
        Assert.AreEqual("Unit Test", addedUser.Name);
        Assert.AreEqual(lastLoginDate, addedUser.LastLogin);
    }

    [Test]
    public async Task TestUserGetById()
    {
        var user = CreateUser(nameof(UserControllerTests), "Unit Test");
        var lastLoginDate = DateTime.UtcNow;
        user.LastLogin = lastLoginDate;
        var addResult = await instance.Add(user, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(addResult);
        var addedUser = (User)addResult.Value;
        Assert.IsTrue(addedUser.Id != 0);

        // Get by id
        var getByIdResult = await instance.GetById(addedUser.Id, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(getByIdResult);
        var getByIdUser = (User)getByIdResult.Value;

        Assert.AreEqual(nameof(UserControllerTests), getByIdUser.Login);
        Assert.AreEqual("Unit Test", getByIdUser.Name);
        Assert.AreEqual(lastLoginDate, getByIdUser.LastLogin);
    }

    [Test]
    public async Task TestRemoveUser() 
    {
        var user = CreateUser(nameof(UserControllerTests), "Unit Test");
        var lastLoginDate = DateTime.UtcNow;
        user.LastLogin = lastLoginDate;
        var addResult = await instance.Add(user, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(addResult);
        var addedUser = (User)addResult.Value;
        Assert.IsTrue(addedUser.Id != 0);

        // Remove newly added user
        var removeResult = await instance.Remove(addedUser.Id, CancellationToken.None) as OkResult;
        Assert.NotNull(removeResult);

        // Test that the user is not found any more
        var getByIdResult = await instance.GetById(addedUser.Id, CancellationToken.None) as NotFoundResult;
        Assert.NotNull(getByIdResult);
    }

    [Test]
    public async Task TestUpdateUser() 
    {
        var user = CreateUser(nameof(UserControllerTests), "Unit Test");
        var lastLoginDate = DateTime.UtcNow;
        user.LastLogin = lastLoginDate;
        var addResult = await instance.Add(user, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(addResult);
        var addedUser = (User)addResult.Value;
        
        const string newName = "Matti Meikäläinen";
        addedUser.Name = newName;

        var updateResult = await instance.Update(addedUser, CancellationToken.None) as OkObjectResult;
        Assert.NotNull(updateResult);
        var updatedUser = (User)updateResult.Value;
        Assert.AreEqual(addedUser.Id, updatedUser.Id);
        Assert.AreEqual(newName, updatedUser.Name);
    }

    [Test]
    public void TestAddUserNullLogin()
    {
        var user = CreateUser(null, "name");
        Assert.ThrowsAsync<DbUpdateException>(async () => await instance.Add(user, CancellationToken.None), "Add with null login should throw DbUpdateException");
    }

    [Test]
    public void TestAddUserNullName()
    {
        var user = CreateUser("login", null);
        Assert.ThrowsAsync<DbUpdateException>(async () => await instance.Add(user, CancellationToken.None), "Add with null name should throw DbUpdateException");
    }

    [Test]
    public async Task TestGetByIdNotFound()
    {
        var user = CreateUser("login", "name");
        await instance.Add(user, CancellationToken.None);

        var getByIdResult = await instance.GetById(-1, CancellationToken.None) as NotFoundResult;
        Assert.IsNotNull(getByIdResult);
    }
}