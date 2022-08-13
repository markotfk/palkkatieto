using palkkatietoapi.Model;
public interface IPalkkatietoService 
{
    Task<IList<Palkka>> GetByQuery(PalkkaQuery query, CancellationToken cancellationToken);

    Task<Palkka?> GetById(long id);

    Task<Palkka> Add(Palkka palkka, CancellationToken cancellationToken);

    Task Remove(long palkkaId, CancellationToken cancellationToken);

    Task<Palkka> Update(Palkka palkka, CancellationToken cancellationToken);
}