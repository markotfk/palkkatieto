using Microsoft.EntityFrameworkCore;
using palkkatietoapi.Model;
using palkkatietoapi.Db;

public class PalkkatietoService : IPalkkatietoService
{
    readonly PalkkaDbContext dbContext;

    public PalkkatietoService(PalkkaDbContext context) 
    {
        this.dbContext = context;
    }

    public async Task<Palkka> Add(Palkka palkka, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(palkka, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return palkka;
    }

    public async Task<Palkka?> GetById(long id)
    {
        var ret = await dbContext.Palkat.FindAsync(id);
        return ret;
    }

    public async Task<IList<Palkka>> GetByQuery(PalkkaQuery query, CancellationToken cancellationToken)
    {
        var palkatQuery = dbContext.Palkat.AsNoTracking();
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
        if (!string.IsNullOrWhiteSpace(query.OrderBy)) {
            if (query.OrderBy == nameof(Palkka.Amount)) {
                palkatQuery = query.OrderByDescending ? 
                    palkatQuery.OrderByDescending(p => p.Amount): palkatQuery.OrderBy(p => p.Amount);
            } else if (query.OrderBy == nameof(Palkka.City)) {
                palkatQuery = query.OrderByDescending ? 
                    palkatQuery.OrderByDescending(p => p.City): palkatQuery.OrderBy(p => p.City);
            } else if (query.OrderBy == nameof(Palkka.Company)) {
                palkatQuery = query.OrderByDescending ? 
                    palkatQuery.OrderByDescending(p => p.Company): palkatQuery.OrderBy(p => p.Company);
            } else if (query.OrderBy == nameof(Palkka.CountryCode)) {
                palkatQuery = query.OrderByDescending ? 
                    palkatQuery.OrderByDescending(p => p.CountryCode): palkatQuery.OrderBy(p => p.CountryCode);
            } else if (query.OrderBy == nameof(Palkka.JobRole)) {
                palkatQuery = query.OrderByDescending ? 
                    palkatQuery.OrderByDescending(p => p.JobRole): palkatQuery.OrderBy(p => p.JobRole);
            }
        }
        return await palkatQuery.ToListAsync(cancellationToken);

    }

    public async Task Remove(long id, CancellationToken cancellationToken)
    {
        var entity = await GetById(id);
        if (entity == null) throw new Exception($"{id} not found in palkka db for removal.");
        dbContext.Entry(entity).State = EntityState.Deleted;
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Palkka> Update(Palkka palkka, CancellationToken cancellationToken) 
    {
        var dbEntity = await GetById(palkka.Id);
        if (dbEntity == null) {
            throw new Exception($"Update: entity {palkka.Id} not found");
        }
        MapData(palkka, dbEntity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return dbEntity;
    }

    private void MapData(Palkka from, Palkka to) 
    {
        to.Amount = from.Amount;
        to.City = from.City;
        to.Company = from.Company;
        to.CountryCode = from.CountryCode;
        to.JobRole = from.JobRole;
        to.Modified = DateTime.UtcNow;
    }
}