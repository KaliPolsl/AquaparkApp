// Plik: Services/KoszykService.cs
using AquaparkApp.Data.Models;

namespace AquaparkApp.Services;

public class KoszykService : IKoszykService
{
    private readonly List<OfertaCennikowa> _pozycje = new();
    public List<OfertaCennikowa> Pozycje => _pozycje;

    public decimal Suma => _pozycje.Sum(p => p.CenaPodstawowa);

    public event Action? OnChange;

    public void DodajDoKoszyka(OfertaCennikowa oferta)
    {
        _pozycje.Add(oferta);
        NotifyStateChanged();
    }

    public void UsunZKoszyka(OfertaCennikowa oferta)
    {
        _pozycje.Remove(oferta);
        NotifyStateChanged();
    }

    public void WyczyscKoszyk()
    {
        _pozycje.Clear();
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}