using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data.Models;

[Table("ProduktZakupiony")]
[Index("KlientId", Name = "IX_ProduktZakupiony_Klient")]
[Index("OfertaId", Name = "IX_ProduktZakupiony_Oferta")]
public partial class ProduktZakupiony
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("klient_id")]
    public int KlientId { get; set; }

    [Column("oferta_id")]
    public int OfertaId { get; set; }

    [Column("znizka_id")]
    public int? ZnizkaId { get; set; }

    [Column("cenaZakupu", TypeName = "decimal(10, 2)")]
    public decimal CenaZakupu { get; set; }

    [Column("dataZakupu", TypeName = "datetime")]
    public DateTime DataZakupu { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column("pozostaloWejsc")]
    public int? PozostaloWejsc { get; set; }

    [Column("waznyOd", TypeName = "datetime")]
    public DateTime? WaznyOd { get; set; }

    [Column("waznyDo", TypeName = "datetime")]
    public DateTime? WaznyDo { get; set; }

    [ForeignKey("KlientId")]
    [InverseProperty("ProduktyZakupione")]
    public virtual Klient Klient { get; set; } = null!;

    [ForeignKey("OfertaId")]
    [InverseProperty("ProduktyZakupione")]
    public virtual OfertaCennikowa Oferta { get; set; } = null!;

    [InverseProperty("ProduktZakupiony")]
    public virtual ICollection<PozycjaPlatnosci> PozycjePlatnosci { get; set; } = new List<PozycjaPlatnosci>();

    [InverseProperty("ProduktZakupiony")]
    public virtual ICollection<Wizyta> Wizyty { get; set; } = new List<Wizyta>();

    // Wskazuje na kolekcję 'ProduktyZakupione' w klasie 'Znizka'
    [ForeignKey("ZnizkaId")]
    [InverseProperty("ProduktyZakupione")]
    public virtual Znizka? Znizka { get; set; }
}
