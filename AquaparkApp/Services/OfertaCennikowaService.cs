using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Services;

public class OfertaCennikowaService(IDbContextFactory<ApplicationDbContext> dbFactory) : IOfertaCennikowaService
{
    public async Task<List<OfertaCennikowa>> PobierzWszystkieOfertyAsync()
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        return await context.OfertyCennikowe.OrderByDescending(o => o.Id).ToListAsync();
    }

    public async Task<OfertaCennikowa?> PobierzOfertePoIdAsync(int id)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        return await context.OfertyCennikowe.FindAsync(id);
    }

    public async Task DodajOferteAsync(OfertaCennikowa oferta)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        context.OfertyCennikowe.Add(oferta);
        await context.SaveChangesAsync();
    }

    public async Task ZaktualizujOferteAsync(OfertaCennikowa oferta)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        context.OfertyCennikowe.Update(oferta);
        await context.SaveChangesAsync();
    }

    public async Task ZmienStatusAktywnosciOfertyAsync(int id, bool czyAktywna)
    {
        await using var context = await dbFactory.CreateDbContextAsync();
        var oferta = await context.OfertyCennikowe.FindAsync(id);
        if (oferta != null)
        {
            oferta.ObowiazujeDo = czyAktywna ? null : DateTime.UtcNow.AddDays(-1);
            await context.SaveChangesAsync();
        }
    }
}