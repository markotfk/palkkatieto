using Microsoft.EntityFrameworkCore;
using palkkatietoapi.Model;
using palkkatietoapi.Db;

public class PalkkatietoService : IPalkkatietoService
{
    readonly IConfiguration configuration;

    public PalkkatietoService(IConfiguration configuration) {
        this.configuration = configuration;
    }
    public async Task Add(Palkka palkka, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        await palkkaDbContext.AddAsync(palkka, cancellationToken);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }

    public Palkka GetById(long id)
    {
        using var palkkaDbContext = new PalkkaDbContext(configuration);
        var ret = palkkaDbContext.Palkat.AsNoTracking().Where(p => p.Id == id).SingleOrDefault();
        return ret;
    }

    public async Task<IList<Palkka>> GetByQuery(PalkkaQuery query, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
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
        if (!string.IsNullOrWhiteSpace(query.JobRole)) {
            palkatQuery = palkatQuery.Where(p => p.JobRole == query.JobRole);
        }
        if (!string.IsNullOrWhiteSpace(query.City)) {
            palkatQuery = palkatQuery.Where(p => p.City == query.City);
        }
        return await palkatQuery.ToListAsync(cancellationToken);

    }

    public async Task Remove(long palkkaId, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        var entity = await palkkaDbContext.FindAsync<Palkka>(palkkaId, cancellationToken);
        if (entity == null) throw new Exception($"{palkkaId} not found in palkka db for removal.");
        palkkaDbContext.Remove(entity);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }
}