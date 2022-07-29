using Microsoft.EntityFrameworkCore;
using palkkatietoapi.Model;
using palkkatietoapi.Db;

public class PalkkatietoService : IPalkkatietoService
{
    readonly IConfiguration configuration;

    public PalkkatietoService(IConfiguration configuration) 
    {
        this.configuration = configuration;
    }

    public async Task Add(Palkka palkka, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        await palkkaDbContext.AddAsync(palkka, cancellationToken);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Palkka?> GetById(long id, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        var ret = await palkkaDbContext.Palkat.FindAsync(id, cancellationToken);
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

    public async Task Remove(long id, CancellationToken cancellationToken)
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        var entity = await GetById(id, cancellationToken);
        if (entity == null) throw new Exception($"{id} not found in palkka db for removal.");
        palkkaDbContext.Remove(entity);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(Palkka palkka, CancellationToken cancellationToken) 
    {
        await using var palkkaDbContext = new PalkkaDbContext(configuration);
        var dbEntity = await GetById(palkka.Id, cancellationToken);
        if (dbEntity == null) {
            throw new Exception($"Update: entity {palkka.Id} not found");
        }
        MapData(palkka, dbEntity);
        await palkkaDbContext.SaveChangesAsync(cancellationToken);
    }

    private void MapData(Palkka from, Palkka to) 
    {
        to.Amount = from.Amount;
        to.City = from.City;
        to.Company = from.Company;
        to.CountryCode = from.CountryCode;
        to.JobRole = from.JobRole;

    }
}