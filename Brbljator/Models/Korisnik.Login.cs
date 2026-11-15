using Microsoft.AspNetCore.Identity;

namespace Brbljator.Models;

// Nastavak klase Korisnik (vidi Korisnik.cs) – logika prijave.
public partial class Korisnik
{
    public const string PorukaPogresnaPrijava = "Pogrešno korisničko ime ili lozinka.";

    private static readonly PasswordHasher<Korisnik> Hasher = new();

    /// <summary>
    /// SCRUM-40: provjera lozinke i statusa računa.
    /// Vraća true ako se korisnik smije prijaviti; inače false i poruku za prikaz (SCRUM-41).
    /// </summary>
    public bool Login(string lozinka, out string poruka)
    {
        var rezultat = Hasher.VerifyHashedPassword(this, LozinkaHash, lozinka);

        // Ista poruka kao za nepostojećeg korisnika – ne otkrivamo koji računi postoje
        if (rezultat == PasswordVerificationResult.Failed)
        {
            poruka = PorukaPogresnaPrijava;
            return false;
        }

        if (Blokiran)
        {
            poruka = "Vaš račun je blokiran. Obratite se administratoru.";
            return false;
        }

        poruka = "";
        return true;
    }
}
