using AquaparkApp.Models.DTOs;

namespace AquaparkApp.Services;

public interface ITransakcjaService
{
    Task<int> ZrealizujTransakcjeAsync(int klientId, List<PozycjaKoszykaDto> pozycjeKoszyka, string metodaPlatnosci, int? znizkaId);
}