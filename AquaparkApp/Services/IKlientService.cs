// Plik: Services/IKlientService.cs
using AquaparkApp.Data.Models;

namespace AquaparkApp.Services;

public interface IKlientService
{
    Task<List<Klient>> WyszukajKlientowAsync(string searchTerm);
    Task<Klient?> PobierzKlientaPoIdAsync(int klientId);
    // Dodajemy nową metodę do pobierania klienta z pełną historią
    Task<Klient?> PobierzKlientaZHistoriaAsync(int klientId);
    Task<int> DodajKlientaAsync(Klient nowyKlient);
    Task ZaktualizujKlientaAsync(Klient klientDoAktualizacji);
    Task UsunKlientaAsync(int klientId);

    Task<List<ProduktZakupiony>> PobierzDostepneProduktyKlientaAsync(int klientId);
}