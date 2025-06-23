using AquaparkApp.Data.Models;
using AquaparkApp.Models.DTOs;

namespace AquaparkApp.Services;

public interface ITransakcjaService
{
    Task<int> ZrealizujZakupKlientaAsync(string userId, List<OfertaCennikowa> pozycjeKoszyka, string metodaPlatnosci);
    Task<int> ZrealizujTransakcjeAsync(int klientId, List<PozycjaKoszykaDto> pozycjeKoszyka, string metodaPlatnosci, int? znizkaId);

    Task<int> ZrealizujPlatnoscZaKaryAsync(int klientId, int wizytaId, List<Kara> karyDoOplacenia, string metodaPlatnosci);
}