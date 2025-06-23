using AquaparkApp.Data.Models;
using AquaparkApp.Data;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Services
{
    public class KaraService(IDbContextFactory<ApplicationDbContext> dbFactory) : IKaraService
    {
        public async Task NaliczKareAsync(int wizytaId, int typKaryId, decimal kwota, string opis)
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var kara = new Kara
            {
                WizytaId = wizytaId,
                TypKaryId = typKaryId,
                Kwota = kwota,
                Opis = opis,
                DataNaliczenia = DateTime.UtcNow,
                StatusPlatnosci = "Niezapłacono"
            };
            db.Kary.Add(kara);
            await db.SaveChangesAsync();
        }

        public async Task<List<Kara>> PobierzNieoplaconeKaryAsync(int wizytaId)
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            return await db.Kary
                .Where(k => k.WizytaId == wizytaId && k.StatusPlatnosci == "Niezapłacono")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task OznaczKaryJakoOplaconeAsync(List<Kara> kary, int platnoscId)
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            foreach (var kara in kary)
            {
                var karaZBazy = await db.Kary.FindAsync(kara.Id);
                if (karaZBazy != null)
                {
                    karaZBazy.StatusPlatnosci = "Zapłacono";
                }
            }
            await db.SaveChangesAsync();
        }
    }
}
