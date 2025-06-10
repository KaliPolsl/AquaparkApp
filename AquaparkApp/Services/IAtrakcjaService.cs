using AquaparkApp.Data.Models;

namespace AquaparkApp.Services;

public interface IAtrakcjaService
{
    Task<List<Atrakcja>> PobierzWszystkieAtrakcjeAsync();
    Task<List<Atrakcja>> PobierzPolecaneAtrakcjeAsync(int ilosc = 3);

}