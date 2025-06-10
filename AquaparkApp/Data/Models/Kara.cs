using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AquaparkApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("Kara")]
[Index("WizytaId", Name = "IX_Kara_Wizyta")]
public partial class Kara
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("wizyta_id")]
    public int WizytaId { get; set; }

    [Column("typKary_id")]
    public int TypKaryId { get; set; }

    [Column("oferta_id")]
    public int? OfertaId { get; set; }

    [Column("kwota", TypeName = "decimal(10, 2)")]
    public decimal Kwota { get; set; }

    [Column("opis")]
    [StringLength(200)]
    public string? Opis { get; set; }

    [Column("dataNaliczenia", TypeName = "datetime")]
    public DateTime DataNaliczenia { get; set; }

    [Column("statusPlatnosci")]
    [StringLength(20)]
    public string StatusPlatnosci { get; set; } = null!;

    [ForeignKey("OfertaId")]
    [InverseProperty("Kary")] // Wskazuje na kolekcję 'Kary' w 'OfertaCennikowa'
    public virtual OfertaCennikowa? Oferta { get; set; }

    [InverseProperty("Kara")]
    public virtual ICollection<PozycjaPlatnosci> PozycjePlatnosci { get; set; } = new List<PozycjaPlatnosci>();

    [ForeignKey("TypKaryId")]
    [InverseProperty("Kary")] // Wskazuje na kolekcję 'Kary' w 'TypKary'
    public virtual TypKary TypKary { get; set; } = null!;

    [ForeignKey("WizytaId")]
    [InverseProperty("Kary")] // Wskazuje na kolekcję 'Kary' w 'Wizyta'
    public virtual Wizyta Wizyta { get; set; } = null!;
}
