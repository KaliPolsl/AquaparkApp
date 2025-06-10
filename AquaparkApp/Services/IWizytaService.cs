using AquaparkApp.Data.Models;

public interface IWizytaService
{
    Task<Wizyta> ZarejestrujWejscieAsync(string numerOpaski);
    Task<Wizyta> ZarejestrujWyjscieAsync(string numerOpaski);
    // Możemy też dodać metodę do odbijania się na bramkach wewnętrznych
    // Task ZarejestrujPrzejsciePrzezBramkeAsync(string numerOpaski, int bramkaId);
}