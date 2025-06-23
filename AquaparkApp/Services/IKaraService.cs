using AquaparkApp.Data.Models;

namespace AquaparkApp.Services
{
    public interface IKaraService
    {
        Task NaliczKareAsync(int wizytaId, int typKaryId, decimal kwota, string opis);

        Task<List<Kara>> PobierzNieoplaconeKaryAsync(int wizytaId);
        
        Task OznaczKaryJakoOplaconeAsync(List<Kara> kary, int platnoscId);
    }
}
