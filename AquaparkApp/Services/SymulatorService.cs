using AquaparkApp.Data;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AquaparkApp.Services
{
    public enum StatusOpaski
    {
        Dostepna,
        WUzyciu,
        Zniszczona,
        Zgubiona
    }

    public class SymulatorService : ISymulatorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOpaskaService _opaskaService;
        private readonly IWizytaService _wizytaService;

        public SymulatorService(
            ApplicationDbContext context,
            IOpaskaService opaskaService,
            IWizytaService wizytaService)
        {
            _context = context;
            _opaskaService = opaskaService;
            _wizytaService = wizytaService;
        }

        public async Task<(bool success, string message, Wizyta wizyta)> RozpocznijWizyte(string numerOpaski)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(numerOpaski))
                    return (false, "Numer opaski nie może być pusty.", null);

                var opaska = await _context.Opaski
                    .Include(o => o.Klient)
                    .FirstOrDefaultAsync(o => o.NumerOpaski == numerOpaski);

                if (opaska == null)
                    return (false, "Nie znaleziono opaski o podanym numerze.", null);

                if (opaska.Status != StatusOpaski.Dostepna.ToString())
                    return (false, "Opaska jest już w użyciu.", null);

                if (opaska.Klient == null)
                    return (false, "Opaska nie jest przypisana do żadnego klienta.", null);

                var statusWizyty = await _context.StatusyWizyt
                    .FirstOrDefaultAsync(s => s.Nazwa == "Aktywna");

                if (statusWizyty == null)
                    return (false, "Nie znaleziono statusu wizyty.", null);

                // Sprawdź czy klient ma aktywny bilet/karnet
                var aktywnyProdukt = await _context.ProduktyZakupione
                    .Where(p => p.KlientId == opaska.Klient.Id && 
                           p.WaznyDo > DateTime.Now && 
                           p.Status == "Aktywny")
                    .OrderByDescending(p => p.WaznyDo)
                    .FirstOrDefaultAsync();

                if (aktywnyProdukt == null)
                    return (false, "Klient nie posiada aktywnego biletu/karnetu.", null);

                var wizyta = new Wizyta
                {
                    OpaskaId = opaska.Id,
                    KlientId = opaska.Klient.Id,
                    ProduktZakupionyId = aktywnyProdukt.Id,
                    CzasWejscia = DateTime.Now,
                    StatusWizytyId = statusWizyty.Id
                };

                _context.Wizyty.Add(wizyta);
                opaska.Status = StatusOpaski.WUzyciu.ToString();

                await _context.SaveChangesAsync();
                return (true, "Wizyta rozpoczęta pomyślnie.", wizyta);
            }
            catch (Exception ex)
            {
                return (false, $"Wystąpił błąd podczas rozpoczynania wizyty: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)> ZalogujPrzejscieBramki(int wizytaId, int bramkaId)
        {
            try
            {
                var wizyta = await _context.Wizyty
                    .Include(w => w.StatusWizyty)
                    .FirstOrDefaultAsync(w => w.Id == wizytaId);

                if (wizyta == null)
                    return (false, "Nie znaleziono wizyty.");

                if (wizyta.StatusWizyty.Nazwa != "Aktywna")
                    return (false, "Wizyta nie jest aktywna.");

                var bramka = await _context.Bramki.FindAsync(bramkaId);
                if (bramka == null)
                    return (false, "Nie znaleziono bramki.");

                var logDostepu = new LogDostepu
                {
                    WizytaId = wizytaId,
                    BramkaId = bramkaId,
                    CzasZdarzenia = DateTime.Now,
                    TypZdarzenia = "Wejście"
                };

                _context.LogiDostepu.Add(logDostepu);
                await _context.SaveChangesAsync();
                return (true, "Przejście przez bramkę zarejestrowane.");
            }
            catch (Exception ex)
            {
                return (false, $"Wystąpił błąd podczas logowania przejścia: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DodajKare(int wizytaId, int typKaryId, decimal kwota)
        {
            try
            {
                var wizyta = await _context.Wizyty
                    .Include(w => w.StatusWizyty)
                    .FirstOrDefaultAsync(w => w.Id == wizytaId);

                if (wizyta == null)
                    return (false, "Nie znaleziono wizyty.");

                if (wizyta.StatusWizyty.Nazwa != "Aktywna")
                    return (false, "Wizyta nie jest aktywna.");

                var typKary = await _context.TypyKar.FindAsync(typKaryId);
                if (typKary == null)
                    return (false, "Nie znaleziono typu kary.");

                if (kwota <= 0)
                    return (false, "Kwota kary musi być większa od zera.");

                var kara = new Kara
                {
                    WizytaId = wizytaId,
                    TypKaryId = typKaryId,
                    Kwota = kwota,
                    DataNaliczenia = DateTime.Now,
                    StatusPlatnosci = "Niezapłacona"
                };

                _context.Kary.Add(kara);
                await _context.SaveChangesAsync();
                return (true, "Kara została naliczona.");
            }
            catch (Exception ex)
            {
                return (false, $"Wystąpił błąd podczas dodawania kary: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> ZakonczWizyte(int wizytaId)
        {
            try
            {
                var wizyta = await _context.Wizyty
                    .Include(w => w.Opaska)
                    .Include(w => w.StatusWizyty)
                    .FirstOrDefaultAsync(w => w.Id == wizytaId);

                if (wizyta == null)
                    return (false, "Nie znaleziono wizyty.");

                if (wizyta.StatusWizyty.Nazwa != "Aktywna")
                    return (false, "Wizyta nie jest aktywna.");

                var statusWizyty = await _context.StatusyWizyt
                    .FirstOrDefaultAsync(s => s.Nazwa == "Zakończona");

                if (statusWizyty == null)
                    return (false, "Nie znaleziono statusu wizyty.");

                wizyta.CzasWyjscia = DateTime.Now;
                wizyta.StatusWizytyId = statusWizyty.Id;
                wizyta.Opaska.Status = StatusOpaski.Dostepna.ToString();

                await _context.SaveChangesAsync();
                return (true, "Wizyta została zakończona.");
            }
            catch (Exception ex)
            {
                return (false, $"Wystąpił błąd podczas kończenia wizyty: {ex.Message}");
            }
        }

        public async Task<Wizyta> PobierzAktywnaWizyte(int wizytaId)
        {
            return await _context.Wizyty
                .Include(w => w.Opaska)
                .Include(w => w.Klient)
                .Include(w => w.StatusWizyty)
                .FirstOrDefaultAsync(w => w.Id == wizytaId);
        }

        public async Task<decimal> PobierzSumaKar(int wizytaId)
        {
            return await _context.Kary
                .Where(k => k.WizytaId == wizytaId)
                .SumAsync(k => k.Kwota);
        }
    }
} 