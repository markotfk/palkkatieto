using palkkatietoapi.Model;
public interface IPalkkatietoService 
{
    public Task<IList<Palkka>> GetByQuery(PalkkaQuery query, CancellationToken cancellationToken);

    public Task<Palkka?> GetById(long id, CancellationToken cancellationToken);

    public Task Add(Palkka palkka, CancellationToken cancellationToken);

    public Task Remove(long palkkaId, CancellationToken cancellationToken);

    public Task Update(Palkka palkka, CancellationToken cancellationToken);
}