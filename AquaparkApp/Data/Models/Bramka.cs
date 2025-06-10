using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AquaparkApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("Bramka")]
public partial class Bramka
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nazwa")]
    [StringLength(50)]
    public string Nazwa { get; set; } = null!;

    [Column("typBramki")]
    [StringLength(20)]
    public string TypBramki { get; set; } = null!;

    [InverseProperty("Bramka")]
    public virtual ICollection<LogDostepu> LogiDostepu { get; set; } = new List<LogDostepu>();

    [ForeignKey("BramkaId")]
    [InverseProperty("Bramki")] // Wskazuje na kolekcję 'Bramki' w klasie 'Atrakcja'
    public virtual ICollection<Atrakcja> Atrakcje { get; set; } = new List<Atrakcja>();
}
