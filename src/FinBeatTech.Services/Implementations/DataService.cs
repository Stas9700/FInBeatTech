using FinBeatTech.Database;
using FinBeatTech.DTO;
using FinBeatTech.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinBeatTech.Services.Implementations;

public class DataService(IDbContextFactory<FinBeatDbContext> factory) : IDataService
{
    public async Task SaveData(IEnumerable<Dictionary<int, string>> inputData)
    {
        using (var dbContext = await factory.CreateDbContextAsync())
        {
            await dbContext.DataEntries.ExecuteDeleteAsync();
            dbContext.DataEntries.AddRange(inputData.SelectMany(dict => dict.Select(kvp => new Data
                {
                    Code = kvp.Key,
                    Value = kvp.Value
                }))
                .OrderBy(e => e.Code)
                .ToArray());
            
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<IReadOnlyCollection<DataEntry>> GetData(int? codeFilter = null)
    {
        using (var dbContext = await factory.CreateDbContextAsync())
        {
            IQueryable<Data> query = dbContext.DataEntries;
            if (codeFilter.HasValue)
            {
                query = query.Where(e => e.Code == codeFilter);
            }
            
            return query.Select(s => new DataEntry()
            {
               Code = s.Code,
               Value = s.Value
            }).ToArray();
        }
    }
}