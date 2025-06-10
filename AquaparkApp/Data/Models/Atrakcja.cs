using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaparkApp.Data.Models;

[Table("Atrakcja")]
public partial class Atrakcja
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nazwa")]
    [StringLength(100)]
    public string Nazwa { get; set; } = null!;

    [Column("opis")]
    public string? Opis { get; set; }

    [Column("maxOsób")]
    public int MaxOsób { get; set; }

    [Column("wymagaDodatkowejOplaty")]
    public bool WymagaDodatkowejOplaty { get; set; }

    [Column("cenaDodatkowa", TypeName = "decimal(10, 2)")]
    public decimal? CenaDodatkowa { get; set; }

    [ForeignKey("AtrakcjaId")]
    [InverseProperty("Atrakcje")] // Wskazuje na kolekcję 'Atrakcje' w klasie 'Bramka'
    public virtual ICollection<Bramka> Bramki { get; set; } = new List<Bramka>();
}
