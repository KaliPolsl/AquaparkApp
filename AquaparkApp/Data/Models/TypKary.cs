using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("TypKary")]
[Index("Nazwa", Name = "UQ_TypKary_Nazwa", IsUnique = true)]
public partial class TypKary
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nazwa")]
    [StringLength(50)]
    public string Nazwa { get; set; } = null!;

    [Column("opis")]
    [StringLength(255)]
    public string? Opis { get; set; }

    [Column("domyslnaKwota", TypeName = "decimal(10, 2)")]
    public decimal? DomyslnaKwota { get; set; }

    [InverseProperty("TypKary")]
    public virtual ICollection<Kara> Kary { get; set; } = new List<Kara>();
}
