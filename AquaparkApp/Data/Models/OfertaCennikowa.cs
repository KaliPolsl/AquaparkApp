using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AquaparkApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("OfertaCennikowa")]
public partial class OfertaCennikowa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nazwaOferty")]
    [StringLength(100)]
    public string NazwaOferty { get; set; } = null!;

    [Column("typ")]
    [StringLength(50)]
    public string Typ { get; set; } = null!;

    [Column("opis")]
    public string? Opis { get; set; }

    [Column("cenaPodstawowa", TypeName = "decimal(10, 2)")]
    public decimal CenaPodstawowa { get; set; }

    [Column("limitCzasuMinuty")]
    public int? LimitCzasuMinuty { get; set; }

    [Column("liczbaWejsc")]
    public int? LiczbaWejsc { get; set; }

    [Column("karaZaMinutePrzekroczenia", TypeName = "decimal(10, 2)")]
    public decimal? KaraZaMinutePrzekroczenia { get; set; }

    [Column("obowiazujeOd", TypeName = "datetime")]
    public DateTime ObowiazujeOd { get; set; }

    [Column("obowiazujeDo", TypeName = "datetime")]
    public DateTime? ObowiazujeDo { get; set; }

    [InverseProperty("Oferta")]
    public virtual ICollection<Kara> Kary { get; set; } = new List<Kara>();

    [InverseProperty("Oferta")]
    public virtual ICollection<ProduktZakupiony> ProduktyZakupione { get; set; } = new List<ProduktZakupiony>();
}
