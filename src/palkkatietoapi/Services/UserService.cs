using palkkatietoapi.Model;
using palkkatietoapi.Db;
using Microsoft.EntityFrameworkCore;
namespace palkkatietoapi.Services;

public class UserService : IUserService
{
    readonly ILogger<UserService> logger;

    readonly PalkkaDbContext dbContext;

    public UserService(ILogger<UserService> logger, PalkkaDbContext dbContext) 
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    public async Task<User> Add(User user, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> GetById(long id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.FindAsync<User>(id);
        return entity;
        
    }

    public async Task Remove(long id, CancellationToken cancellationToken)
    {
        var entity = await GetById(id, cancellationToken);
        if (entity == null) {
            logger.LogError("GetById: Cannot find user {Id}", id);
            throw new Exception($"Cannot find user {id}.");
        }
        dbContext.Entry(entity).State = EntityState.Deleted;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User> Update(User user, CancellationToken cancellationToken)
    {
        var dbEntity = await GetById(user.Id, cancellationToken);
        if (dbEntity == null) {
            logger.LogError("GetById: Cannot find user {Id}", user.Id);
            throw new Exception($"Update: entity {user.Id} not found");
        }
        MapUserData(user, dbEntity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return dbEntity;
    }

    private void MapUserData(User from, User to) 
    {
        to.LastLogin = from.LastLogin;
        to.Login = from.Login;
        to.Name = from.Name;
        to.Modified = DateTime.UtcNow;
    }
}