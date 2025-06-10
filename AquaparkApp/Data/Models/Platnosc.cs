using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("Platnosc")]
public partial class Platnosc
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("klient_id")]
    public int? KlientId { get; set; }

    [Column("kwotaCalkowita", TypeName = "decimal(10, 2)")]
    public decimal KwotaCalkowita { get; set; }

    [Column("dataPlatnosci", TypeName = "datetime")]
    public DateTime DataPlatnosci { get; set; }

    [Column("metodaPlatnosci")]
    [StringLength(50)]
    public string MetodaPlatnosci { get; set; } = null!;

    [Column("statusPlatnosci")]
    [StringLength(20)]
    public string StatusPlatnosci { get; set; } = null!;

    [ForeignKey("KlientId")]
    [InverseProperty("Platnosci")]
    public virtual Klient? Klient { get; set; }

    [InverseProperty("Platnosc")]
    public virtual ICollection<PozycjaPlatnosci> PozycjePlatnosci { get; set; } = new List<PozycjaPlatnosci>();
}
