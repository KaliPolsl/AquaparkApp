using AquaparkApp.Data.Models;

public interface IWizytaService
{
    Task<Wizyta> ZarejestrujWejscieAsync(string numerOpaski);
    Task<Wizyta> ZarejestrujWyjscieAsync(string numerOpaski);
    // Możemy też dodać metodę do odbijania się na bramkach wewnętrznych
    // Task ZarejestrujPrzejsciePrzezBramkeAsync(string numerOpaski, int bramkaId);

    Task<Wizyta> RozpocznijWizyteAsync(string numerOpaski, int produktZakupionyId);
    Task<Wizyta> ZakonczWizyteAsync(string numerOpaski);
    Task<Wizyta?> PobierzAktywnaWizyteAsync(string numerOpaski);
    Task ZarejestrujPrzejscieBramkaAsync(int wizytaId, int bramkaId, string typZdarzenia, string opis);

    Task ZarejestrujSkorzystanieZAtrakcjiAsync(int wizytaId, int bramkaId, int atrakcjaId, string opis);

}