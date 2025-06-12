using AquaparkApp.Data.Models;
using System.Threading.Tasks;

namespace AquaparkApp.Services
{
    public interface ISymulatorService
    {
        Task<(bool success, string message, Wizyta wizyta)> RozpocznijWizyte(string numerOpaski);
        Task<(bool success, string message)> ZalogujPrzejscieBramki(int wizytaId, int bramkaId);
        Task<(bool success, string message)> DodajKare(int wizytaId, int typKaryId, decimal kwota);
        Task<(bool success, string message)> ZakonczWizyte(int wizytaId);
        Task<Wizyta> PobierzAktywnaWizyte(int wizytaId);
        Task<decimal> PobierzSumaKar(int wizytaId);
    }
} 