using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaparkApp.Shared.Models
{
    public class WydarzenieHarmonogramu
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public DayOfWeek DzienTygodnia { get; set; }
        public TimeSpan GodzinaRozpoczecia { get; set; }
        public TimeSpan GodzinaZakonczenia { get; set; }
        public string Lokalizacja { get; set; } = string.Empty; // np. "Basen Sportowy", "Strefa Saun"
        public string? DodatkoweInfo { get; set; }
    }
}
