using palkkatietoapi.Model;

namespace palkkatietoapi.Services;
public interface IUserService 
{
    Task<User?> GetById(long id, CancellationToken cancellationToken);

    Task<User> Add(User user, CancellationToken cancellationToken);

    Task Remove(long id, CancellationToken cancellationToken);

    Task<User> Update(User user, CancellationToken cancellationToken);

}