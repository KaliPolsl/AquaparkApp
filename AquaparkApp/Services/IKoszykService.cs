// Plik: Services/IKoszykService.cs
using AquaparkApp.Data.Models;

public interface IKoszykService
{
    List<OfertaCennikowa> Pozycje { get; }
    decimal Suma { get; }
    event Action? OnChange; // Zdarzenie informujące o zmianie w koszyku
    void DodajDoKoszyka(OfertaCennikowa oferta);
    void UsunZKoszyka(OfertaCennikowa oferta);
    void WyczyscKoszyk();
}