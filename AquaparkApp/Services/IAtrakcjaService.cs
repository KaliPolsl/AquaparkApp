using AquaparkApp.Data.Models;

namespace AquaparkApp.Services;

public interface IAtrakcjaService
{
    Task<List<Atrakcja>> PobierzWszystkieAtrakcjeAsync();
    Task<List<Atrakcja>> PobierzPolecaneAtrakcjeAsync(int ilosc = 3);

    Task<Atrakcja?> PobierzAtrakcjePoIdAsync(int id);
    Task DodajAtrakcjeAsync(Atrakcja atrakcja);
    Task ZaktualizujAtrakcjeAsync(Atrakcja atrakcja);
    Task UsunAtrakcjeAsync(int id); // Opcjonalnie, jeśli chcesz usuwać

    Task ZaktualizujPowiazaniaZBramkamiAsync(int atrakcjaId, List<int> wybraneBramkiId);

}