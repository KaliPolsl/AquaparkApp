// Plik: ApplicationDbContext.cs
// Lokalizacja: AquaparkApp/Data/

using AquaparkApp.Data.Models; // Upewnij siê, ¿e ten using wskazuje na folder z Twoimi modelami
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Data;

// To jest nasz JEDYNY DbContext dla ca³ej aplikacji.
// Dziedziczy po IdentityDbContext, aby zarz¹dzaæ tabelami logowania (AspNetUsers itp.).
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // =======================================================================
    // KROK 1: Definicje DbSet dla wszystkich Twoich tabel z poprawnym nazewnictwem
    // =======================================================================
    public virtual DbSet<Atrakcja> Atrakcje { get; set; }
    public virtual DbSet<Bramka> Bramki { get; set; }
    public virtual DbSet<Kara> Kary { get; set; }
    public virtual DbSet<Klient> Klienci { get; set; }
    public virtual DbSet<LogDostepu> LogiDostepu { get; set; }
    public virtual DbSet<OfertaCennikowa> OfertyCennikowe { get; set; }
    public virtual DbSet<Opaska> Opaski { get; set; }
    public virtual DbSet<Platnosc> Platnosci { get; set; }
    public virtual DbSet<PozycjaPlatnosci> PozycjePlatnosci { get; set; }
    public virtual DbSet<ProduktZakupiony> ProduktyZakupione { get; set; }
    public virtual DbSet<StatusWizyty> StatusyWizyt { get; set; }
    public virtual DbSet<TypKary> TypyKar { get; set; }
    public virtual DbSet<Wizyta> Wizyty { get; set; }
    public virtual DbSet<Znizka> Znizki { get; set; }

    // =======================================================================
    // KROK 2: Konfiguracja relacji i indeksów w OnModelCreating
    // =======================================================================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // KLUCZOWE: To wywo³anie konfiguruje schemat dla ASP.NET Core Identity.
        // MUSI byæ na pocz¹tku.
        base.OnModelCreating(modelBuilder);

        // --- Konfiguracja dla relacji Wiele-do-Wielu: Bramka <-> Atrakcja ---
        modelBuilder.Entity<Bramka>(entity =>
        {
            entity.HasMany(d => d.Atrakcje).WithMany(p => p.Bramki)
                .UsingEntity<Dictionary<string, object>>(
                    "DostepAtrakcjiBramka",
                    r => r.HasOne<Atrakcja>().WithMany()
                        .HasForeignKey("AtrakcjaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DostepAtrakcjiBramka_Atrakcja"),
                    l => l.HasOne<Bramka>().WithMany()
                        .HasForeignKey("BramkaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DostepAtrakcjiBramka_Bramka"),
                    j =>
                    {
                        j.HasKey("BramkaId", "AtrakcjaId");
                        j.ToTable("DostepAtrakcjiBramka");
                        j.IndexerProperty<int>("BramkaId").HasColumnName("bramka_id");
                        j.IndexerProperty<int>("AtrakcjaId").HasColumnName("atrakcja_id");
                    });
        });

        // --- Konfiguracja dla encji Kara ---
        modelBuilder.Entity<Kara>(entity =>
        {
            // Poprawione 'p.Karas' na 'p.Kary' zgodnie z nazewnictwem w modelach
            entity.HasOne(d => d.Oferta).WithMany(p => p.Kary).HasConstraintName("FK_Kara_OfertaCennikowa");

            entity.HasOne(d => d.TypKary).WithMany(p => p.Kary)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kara_TypKary");

            entity.HasOne(d => d.Wizyta).WithMany(p => p.Kary)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kara_Wizyta");
        });

        // --- Konfiguracja dla encji LogDostepu ---
        modelBuilder.Entity<LogDostepu>(entity =>
        {
            // Poprawione nazwy kolekcji
            entity.HasOne(d => d.Bramka).WithMany(p => p.LogiDostepu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogDostepu_Bramka");

            entity.HasOne(d => d.Wizyta).WithMany(p => p.LogiDostepu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogDostepu_Wizyta");
        });

        // --- Konfiguracja dla encji Platnosc ---
        modelBuilder.Entity<Platnosc>(entity =>
        {
            entity.HasIndex(e => e.KlientId, "IX_Platnosc_Klient").HasFilter("([klient_id] IS NOT NULL)");

            entity.HasOne(d => d.Klient).WithMany(p => p.Platnosci).HasConstraintName("FK_Platnosc_Klient");
        });

        // --- Konfiguracja dla encji PozycjaPlatnosci ---
        modelBuilder.Entity<PozycjaPlatnosci>(entity =>
        {
            entity.HasIndex(e => e.KaraId, "IX_PozycjaPlatnosci_Kara").HasFilter("([kara_id] IS NOT NULL)");
            entity.HasIndex(e => e.ProduktZakupionyId, "IX_PozycjaPlatnosci_Produkt").HasFilter("([produktZakupiony_id] IS NOT NULL)");

            // Poprawione nazwy kolekcji
            entity.HasOne(d => d.Kara).WithMany(p => p.PozycjePlatnosci).HasConstraintName("FK_PozycjaPlatnosci_Kara");

            entity.HasOne(d => d.Platnosc).WithMany(p => p.PozycjePlatnosci)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PozycjaPlatnosci_Platnosc");

            entity.HasOne(d => d.ProduktZakupiony).WithMany(p => p.PozycjePlatnosci).HasConstraintName("FK_PozycjaPlatnosci_ProduktZakupiony");
        });

        // --- Konfiguracja dla encji ProduktZakupiony ---
        modelBuilder.Entity<ProduktZakupiony>(entity =>
        {
            // Poprawione nazwy kolekcji
            entity.HasOne(d => d.Klient).WithMany(p => p.ProduktyZakupione)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProduktZakupiony_Klient");

            entity.HasOne(d => d.Oferta).WithMany(p => p.ProduktyZakupione)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProduktZakupiony_OfertaCennikowa");

            entity.HasOne(d => d.Znizka).WithMany(p => p.ProduktyZakupione).HasConstraintName("FK_ProduktZakupiony_Znizka");
        });

        // --- Konfiguracja dla encji Wizyta ---
        // Zmieniono 'Wizytum' na 'Wizyta'
        modelBuilder.Entity<Wizyta>(entity =>
        {
            entity.ToTable("Wizyta"); // Jawne okreœlenie nazwy tabeli dla pewnoœci

            // Twój kluczowy unikalny indeks dla aktywnej opaski!
            entity.HasIndex(e => e.OpaskaId, "IX_Wizyta_AktywnaOpaskaUnikalna")
                .IsUnique()
                .HasFilter("[statusWizyty_id]=(1)"); // Upewnij siê, ¿e 1 to status "Aktywna"

            // Poprawione nazwy kolekcji
            entity.HasOne(d => d.Klient).WithMany(p => p.Wizyty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wizyta_Klient");

            // Poprawiona relacja z Opask¹ - Opaska ma teraz kolekcjê Wizyt
            entity.HasOne(d => d.Opaska).WithMany(p => p.Wizyty)
                .HasForeignKey(d => d.OpaskaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wizyta_Opaska");

            entity.HasOne(d => d.ProduktZakupiony).WithMany(p => p.Wizyty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wizyta_ProduktZakupiony");

            entity.HasOne(d => d.StatusWizyty).WithMany(p => p.Wizyty)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wizyta_StatusWizyty");
        });
    }
}