
using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AquaparkApp.Services;

public class AtrakcjaService(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IAtrakcjaService
{
    public async Task<List<Atrakcja>> PobierzWszystkieAtrakcjeAsync()
    {

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        // Bezpośrednie zapytanie do bazy danych. Proste i wydajne.
        return await dbContext.Atrakcje.ToListAsync();
    }

    public async Task<List<Atrakcja>> PobierzPolecaneAtrakcjeAsync(int ilosc = 3)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        // Prosty sposób na "losowość" w SQL Server to użycie NEWID()
        return await dbContext.Atrakcje
            .OrderBy(a => Guid.NewGuid()) // To może być wolne na dużych zbiorach, ale idealne na start
            .Take(ilosc)
            .AsNoTracking()
            .ToListAsync();
    }

}