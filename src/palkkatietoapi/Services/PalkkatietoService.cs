using Microsoft.EntityFrameworkCore;
using palkkatietoapi.Model;
using palkkatietoapi.Db;

public class PalkkatietoService : IPalkkatietoService
{
    readonly PalkkaDbContext palkkaDbContext;

    public PalkkatietoService(PalkkaDbContext context) 
    {
        palkkaDbContext = context;
    }

    public async Task Add(Palkka palkka, CancellationToken cancellationToken)
    {
        await palkkaDbContext.AddAsync(palkka, cancellationToken);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }

    public Palkka GetById(long id)
    {
        var ret = palkkaDbContext.Palkat.Where(p => p.Id == id).SingleOrDefault();
        return ret;
    }

    public async Task<IList<Palkka>> GetByQuery(PalkkaQuery query, CancellationToken cancellationToken)
    {
        var palkatQuery = palkkaDbContext.Palkat.AsNoTracking().AsQueryable();
        if (query.UserId != null) {
            palkatQuery = palkatQuery.Where(p => p.User.Id == query.UserId);
        }
        if (query.AmountMin.HasValue) {
            palkatQuery = palkatQuery.Where(p => p.Amount >= query.AmountMin.Value);
        }
        if (query.AmountMax.HasValue) {
            palkatQuery = palkatQuery.Where(p => p.Amount <= query.AmountMax.Value);
        }
        if (!string.IsNullOrWhiteSpace(query.Company)) {
            palkatQuery = palkatQuery.Where(p => p.Company == query.Company);
        }
        if (!string.IsNullOrWhiteSpace(query.CountryCode)) {
            palkatQuery = palkatQuery.Where(p => p.CountryCode == query.CountryCode);
        }
        return await palkatQuery.ToListAsync(cancellationToken);

    }

    public async Task Remove(long palkkaId, CancellationToken cancellationToken)
    {
        var entity = await palkkaDbContext.FindAsync<Palkka>(palkkaId, cancellationToken);
        if (entity == null) throw new Exception($"{palkkaId} not found in palkka db for removal.");
        palkkaDbContext.Remove(entity);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }
}