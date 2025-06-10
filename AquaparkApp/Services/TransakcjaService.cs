using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using AquaparkApp.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Services;

public class TransakcjaService(IDbContextFactory<ApplicationDbContext> dbFactory) : ITransakcjaService
{
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
                    DataZakupu = DateTime.Now,
                    Status = "Nowy",
                    PozostaloWejsc = oferta.LiczbaWejsc,
                    WaznyOd = DateTime.Now
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
                DataPlatnosci = DateTime.Now,
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
}