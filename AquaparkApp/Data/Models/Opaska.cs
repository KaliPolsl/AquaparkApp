using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("Opaska")]
[Index("NumerOpaski", Name = "UQ_Opaska_NumerOpaski", IsUnique = true)]
public partial class Opaska
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("numerOpaski")]
    [StringLength(50)]
    [Unicode(false)]
    public string NumerOpaski { get; set; } = null!;

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column("dataWydania", TypeName = "datetime")]
    public DateTime? DataWydania { get; set; }

    [Column("dataWycofania", TypeName = "datetime")]
    public DateTime? DataWycofania { get; set; }

    [Column("klientId")]
    public int? KlientId { get; set; }

    [ForeignKey("KlientId")]
    [InverseProperty("Opaski")]
    public virtual Klient? Klient { get; set; }

    [InverseProperty("Opaska")]
    public virtual ICollection<Wizyta> Wizyty { get; set; } = new List<Wizyta>();
}
