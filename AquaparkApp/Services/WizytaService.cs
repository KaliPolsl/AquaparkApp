using AquaparkApp.Data.Models;
using AquaparkApp.Data;
using Microsoft.EntityFrameworkCore;

public class WizytaService(IDbContextFactory<ApplicationDbContext> dbFactory) : IWizytaService
{
    public async Task<Wizyta> ZarejestrujWejscieAsync(string numerOpaski)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        // 1. Znajdź opaskę
        var opaska = await dbContext.Opaski.FirstOrDefaultAsync(o => o.NumerOpaski == numerOpaski);
        if (opaska == null) throw new Exception("Nie znaleziono opaski.");
        if (opaska.Status != "Dostępna") throw new Exception($"Opaska jest w statusie '{opaska.Status}', nie można jej użyć do wejścia.");

        // 2. Znajdź aktywny, niewykorzystany produkt dla tej opaski (logika do dopracowania)
        // Na razie zakładamy, że produkt jest powiązany z klientem, a opaska z wizytą.
        // To jest uproszczenie. W realnym systemie, przy sprzedaży, trzeba by przypisać produkt do opaski.
        // Na potrzeby symulatora, znajdźmy ostatni zakupiony produkt dla losowego klienta.

        var produkt = await dbContext.ProduktyZakupione
            .Where(p => p.Status == "Nowy")
            .OrderByDescending(p => p.DataZakupu)
            .FirstOrDefaultAsync();

        if (produkt == null) throw new Exception("Nie znaleziono aktywnego, niewykorzystanego produktu do przypisania.");

        // 3. Stwórz nową wizytę
        var nowaWizyta = new Wizyta
        {
            KlientId = produkt.KlientId,
            OpaskaId = opaska.Id,
            ProduktZakupionyId = produkt.Id,
            CzasWejscia = DateTime.Now,
            StatusWizytyId = 1 // Załóżmy, że 1 to "Aktywna"
        };

        // 4. Zmień statusy
        opaska.Status = "Aktywna";
        produkt.Status = "W użyciu";

        dbContext.Wizyty.Add(nowaWizyta);
        await dbContext.SaveChangesAsync();

        return nowaWizyta;
    }

    public async Task<Wizyta> ZarejestrujWyjscieAsync(string numerOpaski)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        var opaska = await dbContext.Opaski.Include(o => o.Wizyty)
            .FirstOrDefaultAsync(o => o.NumerOpaski == numerOpaski);

        if (opaska == null || opaska.Status != "Aktywna") throw new Exception("Ta opaska nie jest aktualnie w użyciu.");

        var aktywnaWizyta = await dbContext.Wizyty
            .FirstOrDefaultAsync(w => w.OpaskaId == opaska.Id && w.StatusWizytyId == 1);

        if (aktywnaWizyta == null) throw new Exception("Błąd systemu: Nie znaleziono aktywnej wizyty dla tej opaski.");

        // Zmień statusy
        aktywnaWizyta.CzasWyjscia = DateTime.Now;
        aktywnaWizyta.StatusWizytyId = 2; // Załóżmy, że 2 to "Zakończona"
        opaska.Status = "Dostępna"; // Opaska wraca do puli dostępnych

        await dbContext.SaveChangesAsync();
        return aktywnaWizyta;
    }
}