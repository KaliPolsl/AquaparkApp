
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaparkApp.Data.Models;

[Table("Wizyta")] 
[Index("KlientId", Name = "IX_Wizyta_Klient")]
[Index("ProduktZakupionyId", Name = "IX_Wizyta_Produkt")]
public partial class Wizyta 
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("klient_id")]
    public int KlientId { get; set; }

    [Column("opaska_id")]
    public int OpaskaId { get; set; }

    [Column("produktZakupiony_id")]
    public int ProduktZakupionyId { get; set; }

    [Column("czasWejscia", TypeName = "datetime")]
    public DateTime CzasWejscia { get; set; }

    [Column("czasWyjscia", TypeName = "datetime")]
    public DateTime? CzasWyjscia { get; set; }

    [Column("planowanyCzasWyjscia", TypeName = "datetime")]
    public DateTime? PlanowanyCzasWyjscia { get; set; }

    [Column("statusWizyty_id")]
    public int StatusWizytyId { get; set; }

    [InverseProperty("Wizyta")]
    public virtual ICollection<Kara> Kary { get; set; } = new List<Kara>();

    [ForeignKey("KlientId")]
    [InverseProperty("Wizyty")] // Wskazuje na kolekcję 'Wizyty' w 'Klient'
    public virtual Klient Klient { get; set; } = null!;

    [InverseProperty("Wizyta")]
    public virtual ICollection<LogDostepu> LogiDostepu { get; set; } = new List<LogDostepu>();

    [ForeignKey("OpaskaId")]
    [InverseProperty("Wizyty")] // Wskazuje na kolekcję 'Wizyty' w 'Opaska'
    public virtual Opaska Opaska { get; set; } = null!;

    [ForeignKey("ProduktZakupionyId")]
    [InverseProperty("Wizyty")] // Wskazuje na kolekcję 'Wizyty' w 'ProduktZakupiony'
    public virtual ProduktZakupiony ProduktZakupiony { get; set; } = null!;

    [ForeignKey("StatusWizytyId")]
    [InverseProperty("Wizyty")] // Wskazuje na kolekcję 'Wizyty' w 'StatusWizyty'
    public virtual StatusWizyty StatusWizyty { get; set; } = null!;
}