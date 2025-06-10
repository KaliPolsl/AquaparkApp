using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Services;

public class OpaskaService(IDbContextFactory<ApplicationDbContext> dbFactory) : IOpaskaService
{
    public async Task<List<Opaska>> PobierzWszystkieOpaskiAsync()
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        return await dbContext.Opaski.OrderByDescending(o => o.DataWydania).ToListAsync();
    }

    public async Task DodajNowaOpaskeAsync(string numerOpaski)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        var czyIstnieje = await dbContext.Opaski.AnyAsync(o => o.NumerOpaski == numerOpaski);
        if (czyIstnieje)
        {
            throw new InvalidOperationException("Opaska o podanym numerze już istnieje w systemie.");
        }

        var nowaOpaska = new Opaska
        {
            NumerOpaski = numerOpaski,
            Status = "Dostępna", // Domyślny status nowej opaski
            DataWydania = DateTime.Now
        };

        await dbContext.Opaski.AddAsync(nowaOpaska);
        await dbContext.SaveChangesAsync();
    }

    public async Task ZmienStatusOpaskiAsync(int opaskaId, string nowyStatus)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        var opaska = await dbContext.Opaski.FindAsync(opaskaId);

        if (opaska != null)
        {
            // Dodatkowa logika biznesowa - np. nie można wycofać opaski, która jest w użyciu
            if (opaska.Status == "Aktywna" && (nowyStatus == "Wycofana" || nowyStatus == "Zgubiona"))
            {
                throw new InvalidOperationException("Nie można zmienić statusu opaski, która jest aktualnie w użyciu (aktywna). Najpierw należy zakończyć powiązaną z nią wizytę.");
            }

            opaska.Status = nowyStatus;

            if (nowyStatus == "Wycofana")
            {
                opaska.DataWycofania = DateTime.Now;
            }

            await dbContext.SaveChangesAsync();
        }
    }
}