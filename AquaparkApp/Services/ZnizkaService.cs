// Plik: Services/ZnizkaService.cs
using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Services;

public class ZnizkaService(IDbContextFactory<ApplicationDbContext> dbFactory) : IZnizkaService
{
    public async Task<List<Znizka>> PobierzAktywneZnizkiAsync()
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        return await dbContext.Znizki
            .Where(z => z.CzyAktywna)
            .AsNoTracking()
            .ToListAsync();
    }
}