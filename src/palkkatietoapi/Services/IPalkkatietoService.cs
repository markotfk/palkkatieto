using palkkatietoapi.Model;
public interface IPalkkatietoService 
{
    public Task<IList<Palkka>> GetByQuery(PalkkaQuery query, CancellationToken cancellationToken);

    public Task<Palkka?> GetById(long id);

    public Task<Palkka> Add(Palkka palkka, CancellationToken cancellationToken);

    public Task Remove(long palkkaId, CancellationToken cancellationToken);

    public Task<Palkka> Update(Palkka palkka, CancellationToken cancellationToken);
}