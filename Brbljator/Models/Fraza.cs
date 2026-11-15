using System.ComponentModel.DataAnnotations;

namespace Brbljator.Models;

// SCRUM-22: tablica Fraza
public class Fraza
{
    [Key]
    public int FrazaID { get; set; }

    // SCRUM-57: strani ključevi UserID i JezikID
    public int UserID { get; set; }
    public Korisnik Korisnik { get; set; } = null!;

    public int JezikID { get; set; }
    public Jezik Jezik { get; set; } = null!;

    [Required, MaxLength(300)]
    public string Tekst { get; set; } = "";       // fraza na stranom jeziku

    [Required, MaxLength(300)]
    public string Znacenje { get; set; } = "";    // prijevod / značenje

    [MaxLength(300)]
    public string? Situacija { get; set; }        // gdje je fraza čuta (trgovina, liječnik...)

    public DateTime DatumUnosa { get; set; } = DateTime.UtcNow;
}
