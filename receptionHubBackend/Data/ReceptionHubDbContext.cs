using Microsoft.EntityFrameworkCore;
using receptionHubBackend.Models;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Data;

public class ReceptionHubDbContext : DbContext
{
    public ReceptionHubDbContext(DbContextOptions<ReceptionHubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Gost> Gosti { get; set; }
    public DbSet<Recepcioner> Recepcioneri { get; set; }
    public DbSet<Soba> Sobe { get; set; }
    public DbSet<Rezervacija> Rezervacije { get; set; }
    public DbSet<ArhiviraniGost> ArhiviraniGosti { get; set; }
    public DbSet<ArhiviranaRezervacija> ArhiviraneRezervacije { get; set; }
    public DbSet<RezervacijaGost> RezervacijaGosti { get; set; }
    public DbSet<Log> Logovi { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enum konverzije
        modelBuilder.Entity<Gost>()
            .Property(g => g.TipGosta)
            .HasConversion<int>();

        modelBuilder.Entity<Recepcioner>()
            .Property(r => r.Pozicija)
            .HasConversion<int>();

        modelBuilder.Entity<Soba>()
            .Property(s => s.TipSobe)
            .HasConversion<int>();

        modelBuilder.Entity<Soba>()
            .Property(s => s.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Rezervacija>()
            .Property(r => r.NacinRezervacije)
            .HasConversion<int>();

        modelBuilder.Entity<Rezervacija>()
            .Property(r => r.Status)
            .HasConversion<int>();

        modelBuilder.Entity<ArhiviraniGost>()
            .Property(ag => ag.TipGosta)
            .HasConversion<int>();

        modelBuilder.Entity<ArhiviraniGost>()
            .Property(ag => ag.RazlogArhiviranja)
            .HasConversion<int>();

        modelBuilder.Entity<ArhiviranaRezervacija>()
            .Property(ar => ar.NacinRezervacije)
            .HasConversion<int>();

        modelBuilder.Entity<ArhiviranaRezervacija>()
            .Property(ar => ar.Status)
            .HasConversion<int>();

        modelBuilder.Entity<ArhiviranaRezervacija>()
            .Property(ar => ar.RazlogArhiviranja)
            .HasConversion<int>();

        modelBuilder.Entity<Log>()
            .Property(l => l.Tip)
            .HasConversion<int>();

        // Veze za RezervacijaGost (many-to-many)
        modelBuilder.Entity<RezervacijaGost>()
            .HasOne(rg => rg.Rezervacija)
            .WithMany(r => r.GostiURezervaciji)
            .HasForeignKey(rg => rg.RezervacijaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RezervacijaGost>()
            .HasOne(rg => rg.Gost)
            .WithMany(g => g.RezervacijeGosta)
            .HasForeignKey(rg => rg.GostId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RezervacijaGost>()
            .HasIndex(rg => new { rg.RezervacijaId, rg.GostId })
            .IsUnique();

        modelBuilder.Entity<RezervacijaGost>()
            .HasIndex(rg => rg.GostId);

        // Veze za Rezervacija
        modelBuilder.Entity<Rezervacija>()
            .HasOne(r => r.Soba)
            .WithMany()
            .HasForeignKey(r => r.SobaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rezervacija>()
            .HasOne(r => r.Recepcioner)
            .WithMany()
            .HasForeignKey(r => r.RecepcionerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Veza za arhivirane podatke
        modelBuilder.Entity<ArhiviraniGost>()
            .HasOne<ArhiviranaRezervacija>()
            .WithMany()
            .HasForeignKey(ag => ag.ArhiviranaRezervacijaId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indeksi
        modelBuilder.Entity<Gost>()
            .HasIndex(g => g.BrojTelefona)
            .HasDatabaseName("IX_Gost_BrojTelefona");

        modelBuilder.Entity<Gost>()
            .HasIndex(g => g.Email)
            .HasDatabaseName("IX_Gost_Email");

        modelBuilder.Entity<Recepcioner>()
            .HasIndex(r => r.KorisnickoIme)
            .IsUnique()
            .HasDatabaseName("IX_Recepcioner_KorisnickoIme");

        modelBuilder.Entity<Recepcioner>()
            .HasIndex(r => r.Email)
            .IsUnique()
            .HasDatabaseName("IX_Recepcioner_Email");

        modelBuilder.Entity<Soba>()
            .HasIndex(s => s.BrojSobe)
            .IsUnique()
            .HasDatabaseName("IX_Soba_BrojSobe");

        modelBuilder.Entity<Soba>()
            .HasIndex(s => s.Status)
            .HasDatabaseName("IX_Soba_Status");

        modelBuilder.Entity<Rezervacija>()
            .HasIndex(r => r.BrojRezervacije)
            .IsUnique()
            .HasDatabaseName("IX_Rezervacija_BrojRezervacije");

        modelBuilder.Entity<Rezervacija>()
            .HasIndex(r => new { r.DatumDolaska, r.DatumOdlaska })
            .HasDatabaseName("IX_Rezervacija_Datumi");

        modelBuilder.Entity<Rezervacija>()
            .HasIndex(r => r.SobaId)
            .HasDatabaseName("IX_Rezervacija_SobaId");

        modelBuilder.Entity<Rezervacija>()
            .HasIndex(r => r.Status)
            .HasDatabaseName("IX_Rezervacija_Status");

        modelBuilder.Entity<ArhiviraniGost>()
            .HasIndex(ag => ag.OriginalniGostId)
            .HasDatabaseName("IX_ArhiviraniGost_OriginalniId");

        modelBuilder.Entity<ArhiviraniGost>()
            .HasIndex(ag => ag.DatumArhiviranja)
            .HasDatabaseName("IX_ArhiviraniGost_DatumArhiviranja");

        modelBuilder.Entity<ArhiviranaRezervacija>()
            .HasIndex(ar => ar.OriginalnaRezervacijaId)
            .HasDatabaseName("IX_ArhiviranaRezervacija_OriginalniId");

        modelBuilder.Entity<ArhiviranaRezervacija>()
            .HasIndex(ar => ar.OriginalniGostId)
            .HasDatabaseName("IX_ArhiviranaRezervacija_OriginalniGostId");

        modelBuilder.Entity<ArhiviranaRezervacija>()
            .HasIndex(ar => ar.DatumArhiviranja)
            .HasDatabaseName("IX_ArhiviranaRezervacija_DatumArhiviranja");

        modelBuilder.Entity<Log>()
            .HasIndex(l => l.Vrijeme);

        modelBuilder.Entity<Log>()
            .HasIndex(l => l.Tip);

        // Default vrijednosti
        modelBuilder.Entity<Gost>()
            .Property(g => g.VIPGost)
            .HasDefaultValue(false);

        modelBuilder.Entity<Gost>()
            .Property(g => g.VrijemeKreiranja)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Soba>()
            .Property(s => s.ImaTv)
            .HasDefaultValue(true);

        modelBuilder.Entity<Soba>()
            .Property(s => s.ImaKlimu)
            .HasDefaultValue(true);

        modelBuilder.Entity<Soba>()
            .Property(s => s.ImaWiFi)
            .HasDefaultValue(true);

        modelBuilder.Entity<Soba>()
            .Property(s => s.Status)
            .HasDefaultValue(StatusSobe.Slobodna);

        modelBuilder.Entity<Rezervacija>()
            .Property(r => r.BrojOdraslih)
            .HasDefaultValue(1);

        modelBuilder.Entity<Rezervacija>()
            .Property(r => r.BrojDjece)
            .HasDefaultValue(0);

        modelBuilder.Entity<Rezervacija>()
            .Property(r => r.VrijemeKreiranja)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Recepcioner>()
            .Property(r => r.Aktivan)
            .HasDefaultValue(true);

        modelBuilder.Entity<Recepcioner>()
            .Property(r => r.DatumKreiranjaRacuna)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Check constraints
        modelBuilder.Entity<Rezervacija>()
            .ToTable(t => t.HasCheckConstraint("CK_Rezervacija_Datumi",
                "\"DatumOdlaska\" > \"DatumDolaska\""));

        modelBuilder.Entity<Soba>()
            .ToTable(t => t.HasCheckConstraint("CK_Soba_Cijena",
                "\"CijenaPoNociBAM\" >= 0"));

        modelBuilder.Entity<Rezervacija>()
            .ToTable(t => t.HasCheckConstraint("CK_Rezervacija_Gosti",
                "\"BrojOdraslih\" + \"BrojDjece\" > 0"));
    }
}