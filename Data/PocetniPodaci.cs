using Brbljator.Models;
using Microsoft.AspNetCore.Identity;

namespace Brbljator.Data;

// Testni korisnici da se prijava može isprobati odmah nakon git clone.
// Registracija (SCRUM-13) dolazi u idućem sprintu.
public static class PocetniPodaci
{
    public static void Dodaj(BrbljatorDbContext db)
    {
        if (db.Korisnici.Any()) return;   // već postoje – ne dodaj ponovno

        var hasher = new PasswordHasher<Korisnik>();

        var admin = new Korisnik { KorisnickoIme = "admin", Email = "admin@brbljator.test", IsAdmin = true };
        admin.LozinkaHash = hasher.HashPassword(admin, "admin123");

        var test = new Korisnik { KorisnickoIme = "test", Email = "test@brbljator.test" };
        test.LozinkaHash = hasher.HashPassword(test, "test123");

        db.Korisnici.AddRange(admin, test);
        db.SaveChanges();
    }
}
