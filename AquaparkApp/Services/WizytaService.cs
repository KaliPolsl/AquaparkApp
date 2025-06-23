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
            CzasWejscia = DateTime.UtcNow,
            StatusWizytyId = 1, // Załóżmy, że 1 to "Aktywna"

            CzasWyjscia = null, // Jawnie ustawiamy na null
            PlanowanyCzasWyjscia = produkt.Oferta.LimitCzasuMinuty.HasValue
        ? DateTime.UtcNow.AddMinutes(produkt.Oferta.LimitCzasuMinuty.Value)
        : null
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
        aktywnaWizyta.CzasWyjscia = DateTime.UtcNow;
        aktywnaWizyta.StatusWizytyId = 2; // Załóżmy, że 2 to "Zakończona"
        opaska.Status = "Dostępna"; // Opaska wraca do puli dostępnych

        await dbContext.SaveChangesAsync();
        return aktywnaWizyta;
    }

    public async Task<Wizyta> RozpocznijWizyteAsync(string numerOpaski, int produktZakupionyId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var opaska = await db.Opaski.FirstOrDefaultAsync(o => o.NumerOpaski == numerOpaski);
        if (opaska == null) throw new InvalidOperationException("Opaska o podanym numerze nie istnieje w systemie.");
        if (opaska.Status != "Dostępna") throw new InvalidOperationException($"Opaska ma status '{opaska.Status}'. Nie można rozpocząć nowej wizyty.");

        var produkt = await db.ProduktyZakupione
            .Include(p => p.Klient)
            .Include(p => p.Oferta)
            .FirstOrDefaultAsync(p => p.Id == produktZakupionyId);

        if (produkt == null) throw new InvalidOperationException("Wybrany bilet/karnet nie został znaleziony.");
        if (produkt.Status != "Nowy") throw new InvalidOperationException("Ten bilet/karnet został już wykorzystany lub jest w użyciu.");


        var nowaWizyta = new Wizyta
        {
            KlientId = produkt.KlientId,
            OpaskaId = opaska.Id,
            ProduktZakupionyId = produkt.Id,
            CzasWejscia = DateTime.UtcNow, // Ustawiamy aktualny czas
            StatusWizytyId = 1, // "Aktywna"
            CzasWyjscia = null, // Jawnie ustawiamy na null
            PlanowanyCzasWyjscia = produkt.Oferta.LimitCzasuMinuty.HasValue
                ? DateTime.UtcNow.AddMinutes(produkt.Oferta.LimitCzasuMinuty.Value)
                : null
        };


        opaska.Status = "Aktywna";
        produkt.Status = "W użyciu";



        if (produkt.PozostaloWejsc.HasValue)
        {
            produkt.PozostaloWejsc--;
        }

        db.Wizyty.Add(nowaWizyta);
        await db.SaveChangesAsync();

        nowaWizyta.Klient = produkt.Klient;
        nowaWizyta.Opaska = opaska;
        nowaWizyta.ProduktZakupiony = produkt;

        return nowaWizyta;
    }

    public async Task<Wizyta> ZakonczWizyteAsync(string numerOpaski)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var aktywnaWizyta = await db.Wizyty
            .Include(w => w.Opaska)
            .Include(w => w.ProduktZakupiony)
            .FirstOrDefaultAsync(w => w.Opaska.NumerOpaski == numerOpaski && w.StatusWizytyId == 1);

        if (aktywnaWizyta == null) throw new InvalidOperationException("Nie znaleziono aktywnej wizyty dla tej opaski.");

        var uzytyProdukt = aktywnaWizyta.ProduktZakupiony;
        if (uzytyProdukt != null)
        {
       
            if (uzytyProdukt.PozostaloWejsc.HasValue)
            {
                // To jest karnet
                if (uzytyProdukt.PozostaloWejsc <= 0)
                {
                    // Właśnie wykorzystano ostatnie wejście
                    uzytyProdukt.Status = "Wykorzystany";
                }
                else
                {
                    // Karnet wciąż ma wejścia, wraca do puli dostępnych
                    uzytyProdukt.Status = "Nowy";
                }
            }
            else
            {
                // To jest bilet jednorazowy
                uzytyProdukt.Status = "Wykorzystany";
            }
            
        }

        // Aktualizujemy wizytę i opaskę
        aktywnaWizyta.CzasWyjscia = DateTime.UtcNow;
        aktywnaWizyta.StatusWizytyId = 2; // "Zakończona"
        aktywnaWizyta.Opaska!.Status = "Dostępna";

        await db.SaveChangesAsync();
        return aktywnaWizyta;
    }

    public async Task<Wizyta?> PobierzAktywnaWizyteAsync(string numerOpaski)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await PobierzAktywnaWizyteAsync(numerOpaski, db);
    }

    private async Task<Wizyta?> PobierzAktywnaWizyteAsync(string numerOpaski, ApplicationDbContext db)
    {
        return await db.Wizyty
            .Include(w => w.Opaska)
            .Include(w => w.Klient)
            .Include(w => w.ProduktZakupiony).ThenInclude(p => p.Oferta)
            .Include(w => w.StatusWizyty)
            .Include(w => w.Kary)
            .FirstOrDefaultAsync(w => w.Opaska.NumerOpaski == numerOpaski && w.StatusWizytyId == 1);
    }

    public async Task ZarejestrujPrzejscieBramkaAsync(int wizytaId, int bramkaId, string typZdarzenia, string opis)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var bramka = await db.Bramki
            .Include(b => b.Atrakcje)
            .FirstOrDefaultAsync(b => b.Id == bramkaId);

        if (bramka != null)
        {
            foreach (var atrakcja in bramka.Atrakcje)
            {
                if (atrakcja.WymagaDodatkowejOplaty && atrakcja.CenaDodatkowa.HasValue)
                {
                    var kara = new Kara
                    {
                        WizytaId = wizytaId,
                        TypKaryId = 3, // <<< Używamy ID=3 dla "Opłata za Atrakcję"
                        Kwota = atrakcja.CenaDodatkowa.Value,
                        Opis = $"Opłata za dostęp: {atrakcja.Nazwa}",
                        DataNaliczenia = DateTime.UtcNow,
                        StatusPlatnosci = "Niezapłacono"
                    };
                    db.Kary.Add(kara);
                }
            }
        }

        var log = new LogDostepu
        {
            WizytaId = wizytaId,
            BramkaId = bramkaId,
            CzasZdarzenia = DateTime.UtcNow,
            TypZdarzenia = typZdarzenia,
            Opis = opis
        };
        db.LogiDostepu.Add(log);

        // Zapisujemy w jednym miejscu zarówno nowy log, jak i potencjalną nową karę
        await db.SaveChangesAsync();
    }

    public async Task ZarejestrujSkorzystanieZAtrakcjiAsync(int wizytaId, int bramkaId, int atrakcjaId, string opis)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        // Pobieramy TYLKO tę jedną, konkretną atrakcję
        var atrakcja = await db.Atrakcje.FindAsync(atrakcjaId);

        if (atrakcja != null)
        {
            // Naliczamy opłatę tylko jeśli ta konkretna atrakcja jest płatna
            if (atrakcja.WymagaDodatkowejOplaty && atrakcja.CenaDodatkowa.HasValue)
            {
                var kara = new Kara
                {
                    WizytaId = wizytaId,
                    TypKaryId = 3, // "Opłata za Atrakcję"
                    Kwota = atrakcja.CenaDodatkowa.Value,
                    Opis = $"Opłata za dostęp: {atrakcja.Nazwa}",
                    DataNaliczenia = DateTime.UtcNow,
                    StatusPlatnosci = "Niezapłacono"
                };
                db.Kary.Add(kara);
            }
        }

        var log = new LogDostepu
        {
            WizytaId = wizytaId,
            CzasZdarzenia = DateTime.UtcNow,
            BramkaId = bramkaId,
            TypZdarzenia = "ATRAKCJA",
            Opis = opis
        };
        db.LogiDostepu.Add(log);

        await db.SaveChangesAsync();
    }


}