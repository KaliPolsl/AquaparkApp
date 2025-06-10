using AquaparkApp.Data.Models;

namespace AquaparkApp.Services;

public interface IOfertaCennikowaService
{
    Task<List<OfertaCennikowa>> PobierzWszystkieOfertyAsync(); // Zmieniamy nazwę na bardziej ogólną
    Task<OfertaCennikowa?> PobierzOfertePoIdAsync(int id);
    Task DodajOferteAsync(OfertaCennikowa oferta);
    Task ZaktualizujOferteAsync(OfertaCennikowa oferta);
    Task ZmienStatusAktywnosciOfertyAsync(int id, bool czyAktywna); // Do "usuwania" przez dezaktywację

}