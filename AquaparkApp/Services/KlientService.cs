// Plik: Services/KlientService.cs
using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Services;

// Zmieniamy to, co wstrzykujemy w konstruktorze!
public class KlientService(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IKlientService
{
    // Nie mamy już pola dbContext. Mamy pole z fabryką.
    public async Task<List<Klient>> WyszukajKlientowAsync(string searchTerm)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new List<Klient>();
        }

        var lowerTerm = searchTerm.ToLower();

        // Sprawdzamy, czy wpisany tekst jest liczbą, aby móc szukać po ID
        bool jestLiczba = int.TryParse(searchTerm, out int id);

        return await dbContext.Klienci
            .Where(k =>
                // Wyszukiwanie po imieniu (bez uwzględniania wielkości liter)
                (k.Imię != null && k.Imię.ToLower().Contains(lowerTerm)) ||

                // Wyszukiwanie po nazwisku (bez uwzględniania wielkości liter)
                (k.Nazwisko != null && k.Nazwisko.ToLower().Contains(lowerTerm)) ||

                // === NOWA CZĘŚĆ: Wyszukiwanie po emailu ===
                (k.Email != null && k.Email.ToLower().Contains(lowerTerm)) ||

                // === NOWA CZĘŚĆ: Wyszukiwanie po ID ===
                // Ten warunek jest spełniony tylko, jeśli 'searchTerm' dało się przekonwertować na liczbę 'id'
                (jestLiczba && k.Id == id)
            )
            .AsNoTracking()
            .Take(20) // Dobra praktyka: ograniczamy liczbę wyników
            .ToListAsync();
    }

    public async Task<Klient?> PobierzKlientaZHistoriaAsync(int klientId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        return await dbContext.Klienci
            .Include(k => k.ProduktyZakupione)
                .ThenInclude(p => p.Oferta)
            .Include(k => k.Wizyty)
                .ThenInclude(w => w.StatusWizyty)
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.Id == klientId);
    }

    // Zaktualizuj WSZYSTKIE pozostałe metody w ten sam sposób!

    public async Task<Klient?> PobierzKlientaPoIdAsync(int klientId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        // Używamy FindAsync, który domyślnie śledzi encje, więc nie dodajemy AsNoTracking()
        return await dbContext.Klienci.FindAsync(klientId);
    }

    public async Task<int> DodajKlientaAsync(Klient nowyKlient)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        await dbContext.Klienci.AddAsync(nowyKlient);
        await dbContext.SaveChangesAsync();
        return nowyKlient.Id;
    }

    public async Task ZaktualizujKlientaAsync(Klient klientDoAktualizacji)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        dbContext.Klienci.Update(klientDoAktualizacji);
        await dbContext.SaveChangesAsync();
    }
    



    public async Task UsunKlientaAsync(int klientId)
    {

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var klient = await dbContext.Klienci.FindAsync(klientId);
        if (klient != null)
        {
            dbContext.Klienci.Remove(klient);
            await dbContext.SaveChangesAsync();
        }
    }
}