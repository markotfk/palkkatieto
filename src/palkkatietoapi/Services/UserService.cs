
using palkkatietoapi.Model;
using palkkatietoapi.Db;

namespace palkkatietoapi.Services;

public class UserService : IUserService
{
    readonly ILogger<UserService> logger;
    readonly IConfiguration configuration;

    public UserService(ILogger<UserService> logger, IConfiguration configuration) 
    {
        this.logger = logger;
        this.configuration = configuration;
    }

    public async Task Add(User user, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        palkkaDbContext.Add(user);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetById(long id, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        var entity = await palkkaDbContext.FindAsync<User>(id);
        if (entity == null) {
            logger.LogError("GetById: Cannot find user {Id}", id);
            throw new Exception($"Cannot find user {id}.");
        } 
        return entity;
        
    }

    public async Task Remove(long id, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        var entity = await GetById(id, cancellationToken);
        if (entity == null) {
            logger.LogError("GetById: Cannot find user {Id}", id);
            throw new Exception($"Cannot find user {id}.");
        } 
        palkkaDbContext.Remove<User>(entity);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(User user, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        var dbEntity = await GetById(user.Id, cancellationToken);
        if (dbEntity == null) {
            logger.LogError("GetById: Cannot find user {Id}", user.Id);
            throw new Exception($"Update: entity {user.Id} not found");
        }
        MapUserData(user, dbEntity);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }

    private void MapUserData(User from, User to) 
    {
        to.LastLogin = from.LastLogin;
        to.Login = from.Login;
        to.Name = from.Name;
    }
}