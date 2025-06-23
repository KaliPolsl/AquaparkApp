using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using AquaparkApp.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Services;

public class TransakcjaService(IDbContextFactory<ApplicationDbContext> dbFactory) : ITransakcjaService
{
    public async Task<int> ZrealizujZakupKlientaAsync(string userId, List<OfertaCennikowa> pozycjeKoszyka, string metodaPlatnosci)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();

        // Znajdź naszego Klienta na podstawie ID użytkownika Identity
        var klient = await dbContext.Klienci.FirstOrDefaultAsync(k => k.Email == userId); // Zakładamy, że email to username
        if (klient == null) throw new Exception("Nie znaleziono profilu klienta dla zalogowanego użytkownika.");

        // Używamy transakcji, aby zapewnić spójność danych
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            var produktyDoZapisu = pozycjeKoszyka.Select(oferta => new ProduktZakupiony
            {
                KlientId = klient.Id,
                OfertaId = oferta.Id,
                CenaZakupu = oferta.CenaPodstawowa,
                DataZakupu = DateTime.UtcNow,
                Status = "Nowy",
                PozostaloWejsc = oferta.LiczbaWejsc,
                WaznyOd = DateTime.UtcNow,
                WaznyDo = DateTime.UtcNow.AddYears(1)
            }).ToList();

            await dbContext.ProduktyZakupione.AddRangeAsync(produktyDoZapisu);
            await dbContext.SaveChangesAsync();

            var platnosc = new Platnosc
            {
                KlientId = klient.Id,
                KwotaCalkowita = pozycjeKoszyka.Sum(p => p.CenaPodstawowa),
                DataPlatnosci = DateTime.UtcNow,
                MetodaPlatnosci = metodaPlatnosci,
                StatusPlatnosci = "Oczekuje" // Np. oczekuje na płatność online
            };

            await dbContext.Platnosci.AddAsync(platnosc);
            await dbContext.SaveChangesAsync();

            // ... (logika tworzenia PozycjiPłatności) ...

            await transaction.CommitAsync();
            return platnosc.Id;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }



    public async Task<int> ZrealizujTransakcjeAsync(int klientId, List<PozycjaKoszykaDto> pozycjeKoszyka, string metodaPlatnosci, int? znizkaId)
    {
        await using var dbContext = await dbFactory.CreateDbContextAsync();
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var ofertaIds = pozycjeKoszyka.Select(p => p.OfertaId).ToList();
            var oferty = await dbContext.OfertyCennikowe.Where(o => ofertaIds.Contains(o.Id)).ToListAsync();
            var znizka = znizkaId.HasValue ? await dbContext.Znizki.FindAsync(znizkaId.Value) : null;

            var produktyDoZapisu = new List<ProduktZakupiony>();
            decimal kwotaFinalna = 0;

            foreach (var pozycja in pozycjeKoszyka)
            {
                var oferta = oferty.First(o => o.Id == pozycja.OfertaId);

                // --- POPRAWIONA LOGIKA OBLICZANIA CENY ---
                decimal cenaPoZnizce = oferta.CenaPodstawowa;
                if (znizka != null)
                {
                    if (znizka.TypZnizki == "Procentowa")
                    {
                        cenaPoZnizce = oferta.CenaPodstawowa * (1 - (znizka.Wartosc / 100));
                    }
                    else if (znizka.TypZnizki == "Kwotowa")
                    {
                        // Zniżka kwotowa na pojedynczy produkt - rzadziej, ale możliwe
                        cenaPoZnizce = Math.Max(0, oferta.CenaPodstawowa - znizka.Wartosc);
                    }
                }

                var produkt = new ProduktZakupiony
                {
                    KlientId = klientId,
                    OfertaId = oferta.Id,
                    ZnizkaId = znizkaId,
                    CenaZakupu = cenaPoZnizce, // Zapisujemy cenę FAKTYCZNIE zapłaconą za produkt
                    DataZakupu = DateTime.UtcNow,
                    Status = "Nowy",
                    PozostaloWejsc = oferta.LiczbaWejsc,
                    WaznyOd = DateTime.UtcNow, // Bilet/karnet jest ważny od momentu zakupu
                    WaznyDo = DateTime.UtcNow.AddYears(1) // Ważność ustawiona na rok od dzisiaj
                };
                produktyDoZapisu.Add(produkt);
                kwotaFinalna += cenaPoZnizce; // Sumujemy ceny po zniżce
            }

            await dbContext.ProduktyZakupione.AddRangeAsync(produktyDoZapisu);
            await dbContext.SaveChangesAsync();

            var platnosc = new Platnosc
            {
                KlientId = klientId,
                KwotaCalkowita = kwotaFinalna,
                DataPlatnosci = DateTime.UtcNow,
                MetodaPlatnosci = metodaPlatnosci,
                StatusPlatnosci = "Zapłacono"
            };
            await dbContext.Platnosci.AddAsync(platnosc);
            await dbContext.SaveChangesAsync();

            var pozycjePlatnosci = produktyDoZapisu.Select(p => new PozycjaPlatnosci
            {
                PlatnoscId = platnosc.Id,
                ProduktZakupionyId = p.Id,
                OpisPozycji = oferty.First(o => o.Id == p.OfertaId).NazwaOferty,
                KwotaPozycji = p.CenaZakupu
            }).ToList();
            await dbContext.PozycjePlatnosci.AddRangeAsync(pozycjePlatnosci);
            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return platnosc.Id;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<int> ZrealizujPlatnoscZaKaryAsync(int klientId, int wizytaId, List<Kara> karyDoOplacenia, string metodaPlatnosci)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();
        try
        {
            var kwotaCalkowita = karyDoOplacenia.Sum(k => k.Kwota);

            // 1. Stwórz Płatność
            var platnosc = new Platnosc
            {
                KlientId = klientId,
                KwotaCalkowita = kwotaCalkowita,
                DataPlatnosci = DateTime.UtcNow,
                MetodaPlatnosci = metodaPlatnosci,
                StatusPlatnosci = "Zapłacono"
            };
            db.Platnosci.Add(platnosc);
            await db.SaveChangesAsync();

            // 2. Stwórz PozycjePłatności, łącząc kary z płatnością
            var pozycjePlatnosci = karyDoOplacenia.Select(k => new PozycjaPlatnosci
            {
                PlatnoscId = platnosc.Id,
                KaraId = k.Id,
                OpisPozycji = k.Opis ?? "Kara",
                KwotaPozycji = k.Kwota
            }).ToList();
            await db.PozycjePlatnosci.AddRangeAsync(pozycjePlatnosci);

            // 3. Zaktualizuj statusy kar
            foreach (var kara in karyDoOplacenia)
            {
                var karaZBazy = await db.Kary.FindAsync(kara.Id);
                if (karaZBazy != null) karaZBazy.StatusPlatnosci = "Zapłacono";
            }

            await db.SaveChangesAsync();
            await transaction.CommitAsync();

            return platnosc.Id;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

}