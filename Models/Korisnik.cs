using System.ComponentModel.DataAnnotations;

namespace Brbljator.Models;

// SCRUM-17: tablica Korisnik
// "partial" znači da se klasa može nastaviti u drugoj datoteci (Korisnik.Login.cs radi Ivan).
public partial class Korisnik
{
    [Key]                                   // SCRUM-47: primarni ključ IDUser
    public int IDUser { get; set; }

    // SCRUM-46: stupci i tipovi
    [Required, MaxLength(50)]
    public string KorisnickoIme { get; set; } = "";

    [Required, MaxLength(100)]
    public string Email { get; set; } = "";

    [Required]
    public string LozinkaHash { get; set; } = "";   // nikad čista lozinka

    public bool IsAdmin { get; set; }

    public bool Blokiran { get; set; }

    public DateTime DatumRegistracije { get; set; } = DateTime.UtcNow;

    public List<Fraza> Fraze { get; set; } = new();
}
