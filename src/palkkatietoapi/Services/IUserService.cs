using palkkatietoapi.Model;

namespace palkkatietoapi.Services;
public interface IUserService 
{
    public Task<User?> GetById(long id, CancellationToken cancellationToken);

    public Task Add(User user, CancellationToken cancellationToken);

    public Task Remove(long id, CancellationToken cancellationToken);

    public Task Update(User user, CancellationToken cancellationToken);

}