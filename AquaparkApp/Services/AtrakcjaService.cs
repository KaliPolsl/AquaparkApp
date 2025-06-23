
using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AquaparkApp.Services;

public class AtrakcjaService(IDbContextFactory<ApplicationDbContext> dbFactory) : IAtrakcjaService
{
    public async Task<List<Atrakcja>> PobierzWszystkieAtrakcjeAsync()
    {

        await using var dbContext = await dbFactory.CreateDbContextAsync();
        // Bezpośrednie zapytanie do bazy danych. Proste i wydajne.
        return await dbContext.Atrakcje.ToListAsync();
    }

    public async Task<List<Atrakcja>> PobierzPolecaneAtrakcjeAsync(int ilosc = 3)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        // Prosty sposób na "losowość" w SQL Server to użycie NEWID()
        return await dbContext.Atrakcje
            .OrderBy(a => Guid.NewGuid()) // To może być wolne na dużych zbiorach, ale idealne na start
            .Take(ilosc)
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task<Atrakcja?> PobierzAtrakcjePoIdAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Atrakcje
            .Include(a => a.Bramki) // <-- WAŻNA ZMIANA
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task DodajAtrakcjeAsync(Atrakcja atrakcja)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.Atrakcje.Add(atrakcja);
        await db.SaveChangesAsync();
    }

    public async Task ZaktualizujAtrakcjeAsync(Atrakcja atrakcja)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        db.Atrakcje.Update(atrakcja);
        await db.SaveChangesAsync();
    }

    public async Task UsunAtrakcjeAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var atrakcja = await db.Atrakcje.FindAsync(id);
        if (atrakcja != null)
        {
            db.Atrakcje.Remove(atrakcja);
            await db.SaveChangesAsync();
        }
    }

    public async Task ZaktualizujPowiazaniaZBramkamiAsync(int atrakcjaId, List<int> wybraneBramkiId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        // Znajdź atrakcję i załaduj jej aktualne powiązania z bramkami
        var atrakcja = await db.Atrakcje
            .Include(a => a.Bramki)
            .FirstOrDefaultAsync(a => a.Id == atrakcjaId);

        if (atrakcja == null) return;

        // Usuń stare powiązania
        atrakcja.Bramki.Clear();

        // Dodaj nowe powiązania
        if (wybraneBramkiId != null && wybraneBramkiId.Any())
        {
            var bramkiDoDodania = await db.Bramki
                .Where(b => wybraneBramkiId.Contains(b.Id))
                .ToListAsync();

            foreach (var bramka in bramkiDoDodania)
            {
                atrakcja.Bramki.Add(bramka);
            }
        }

        await db.SaveChangesAsync();
    }

}