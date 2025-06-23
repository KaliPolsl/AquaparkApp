// Plik: LogDostepu.cs
// Lokalizacja: AquaparkApp/Data/Models/

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AquaparkApp.Data.Models;

[Table("LogDostepu")]
[Index("BramkaId", Name = "IX_LogDostepu_Bramka")]
[Index("WizytaId", Name = "IX_LogDostepu_Wizyta")]
// Usunąłem indeks na CzasZdarzenia, chyba że jest Ci naprawdę potrzebny,
// bo rzadko indeksuje się same daty bez dodatkowych kolumn.
public partial class LogDostepu
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("wizyta_id")]
    public int WizytaId { get; set; }

    [Column("bramka_id")]
    public int BramkaId { get; set; }

    [Column("czasZdarzenia", TypeName = "datetime")]
    public DateTime CzasZdarzenia { get; set; }

    [Column("typZdarzenia")]
    [StringLength(20)]
    public string TypZdarzenia { get; set; } = null!;

    [Column("opis")]
    [StringLength(200)] // Dłuższe pole na czytelny opis
    public string? Opis { get; set; }

    [ForeignKey("BramkaId")]
    [InverseProperty("LogiDostepu")]
    public virtual Bramka Bramka { get; set; } = null!;


    [ForeignKey("WizytaId")]
    [InverseProperty("LogiDostepu")]
    public virtual Wizyta Wizyta { get; set; } = null!;
}