using Brbljator.Models;
using Microsoft.EntityFrameworkCore;

namespace Brbljator.Data;

public class BrbljatorDbContext(DbContextOptions<BrbljatorDbContext> options) : DbContext(options)
{
    public DbSet<Korisnik> Korisnici => Set<Korisnik>();
    public DbSet<Jezik> Jezici => Set<Jezik>();
    public DbSet<Fraza> Fraze => Set<Fraza>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Nazivi tablica kao u Jiri (inače bi EF koristio imena DbSet-ova: Korisnici, Jezici, Fraze)
        modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
        modelBuilder.Entity<Jezik>().ToTable("Jezik");
        modelBuilder.Entity<Fraza>().ToTable("Fraza");

        // Korisničko ime i e-mail moraju biti jedinstveni
        modelBuilder.Entity<Korisnik>().HasIndex(k => k.KorisnickoIme).IsUnique();
        modelBuilder.Entity<Korisnik>().HasIndex(k => k.Email).IsUnique();
        modelBuilder.Entity<Jezik>().HasIndex(j => j.Kod).IsUnique();

        // SCRUM-57: Fraza.UserID -> Korisnik.IDUser (brisanjem korisnika brišu se i njegove fraze)
        modelBuilder.Entity<Fraza>()
            .HasOne(f => f.Korisnik)
            .WithMany(k => k.Fraze)
            .HasForeignKey(f => f.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        // SCRUM-57: Fraza.JezikID -> Jezik.JezikID (jezik se ne može obrisati dok ima fraza)
        modelBuilder.Entity<Fraza>()
            .HasOne(f => f.Jezik)
            .WithMany(j => j.Fraze)
            .HasForeignKey(f => f.JezikID)
            .OnDelete(DeleteBehavior.Restrict);

        // SCRUM-58: početni jezici
        modelBuilder.Entity<Jezik>().HasData(
            new Jezik { JezikID = 1, Naziv = "Engleski", Kod = "en" },
            new Jezik { JezikID = 2, Naziv = "Njemački", Kod = "de" },
            new Jezik { JezikID = 3, Naziv = "Talijanski", Kod = "it" },
            new Jezik { JezikID = 4, Naziv = "Španjolski", Kod = "es" },
            new Jezik { JezikID = 5, Naziv = "Francuski", Kod = "fr" });
    }
}
