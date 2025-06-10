using AquaparkApp.Data.Models;

namespace AquaparkApp.Services;

public interface IOpaskaService
{
    Task<List<Opaska>> PobierzWszystkieOpaskiAsync();
    Task DodajNowaOpaskeAsync(string numerOpaski);
    Task ZmienStatusOpaskiAsync(int opaskaId, string nowyStatus);
}