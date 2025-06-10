using AquaparkApp.Data.Models;

namespace AquaparkApp.Services;

public interface IZnizkaService
{
    Task<List<Znizka>> PobierzAktywneZnizkiAsync();
}