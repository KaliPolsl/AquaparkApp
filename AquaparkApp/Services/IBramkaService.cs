using AquaparkApp.Data.Models;

namespace AquaparkApp.Services
{
    public interface IBramkaService
    {
        Task<List<Bramka>> PobierzBramkiZAtrakcjamiAsync();

        Task<List<Bramka>> PobierzWszystkieBramkiAsync();
    }
}
