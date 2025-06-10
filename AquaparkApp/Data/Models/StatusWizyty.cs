using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AquaparkApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("StatusWizyty")]
[Index("Nazwa", Name = "UQ_StatusWizyty_Nazwa", IsUnique = true)]
public partial class StatusWizyty
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nazwa")]
    [StringLength(30)]
    public string Nazwa { get; set; } = null!;

    [Column("opis")]
    [StringLength(255)]
    public string? Opis { get; set; }

    [InverseProperty("StatusWizyty")]
    public virtual ICollection<Wizyta> Wizyty { get; set; } = new List<Wizyta>();
}
