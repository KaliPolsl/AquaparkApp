using AquaparkApp.Data.Models;
using AquaparkApp.Data;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Services
{
    public class BramkaService(IDbContextFactory<ApplicationDbContext> dbFactory) : IBramkaService
    {
        public async Task<List<Bramka>> PobierzBramkiZAtrakcjamiAsync()
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            return await db.Bramki
                .Include(b => b.Atrakcje) // KLUCZ: Dołączamy powiązane atrakcje
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Bramka>> PobierzWszystkieBramkiAsync()
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            return await db.Bramki
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
