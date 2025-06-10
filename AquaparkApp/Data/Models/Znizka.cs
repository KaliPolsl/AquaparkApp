using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("Znizka")]
[Index("KodZnizki", Name = "UQ_Znizka_KodZnizki", IsUnique = true)]
public partial class Znizka
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("kodZnizki")]
    [StringLength(20)]
    public string KodZnizki { get; set; } = null!;

    [Column("opis")]
    [StringLength(100)]
    public string Opis { get; set; } = null!;

    [Column("typZnizki")]
    [StringLength(15)]
    public string TypZnizki { get; set; } = null!;

    [Column("wartosc", TypeName = "decimal(10, 2)")]
    public decimal Wartosc { get; set; }

    [Column("czyAktywna")]
    public bool CzyAktywna { get; set; }

    [InverseProperty("Znizka")]
    public virtual ICollection<ProduktZakupiony> ProduktyZakupione { get; set; } = new List<ProduktZakupiony>();
}
