using AquaparkApp.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AquaparkApp.Data.Models;

[Table("Klient")]
public partial class Klient
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("imię")]
    [StringLength(50)]
    public string? Imię { get; set; }

    [Column("nazwisko")]
    [StringLength(50)]
    public string? Nazwisko { get; set; }

    [Column("nrTelefonu")]
    [StringLength(20)]
    public string? NrTelefonu { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [InverseProperty("Klient")]
    public virtual ICollection<Platnosc> Platnosci { get; set; } = new List<Platnosc>();

    [InverseProperty("Klient")]
    public virtual ICollection<ProduktZakupiony> ProduktyZakupione { get; set; } = new List<ProduktZakupiony>();

    [InverseProperty("Klient")]
    public virtual ICollection<Wizyta> Wizyty { get; set; } = new List<Wizyta>();
}
