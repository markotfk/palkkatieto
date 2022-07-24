using palkkatietoapi.Model;
public interface IPalkkatietoService 
{
    public Task<IList<Palkka>> GetByQuery(PalkkaQuery query, CancellationToken cancellationToken);

    public Palkka GetById(long id);

    public Task Add(Palkka palkka, CancellationToken cancellationToken);

    public Task Remove(long palkkaId, CancellationToken cancellationToken);
}