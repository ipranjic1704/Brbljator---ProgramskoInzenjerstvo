using System.ComponentModel.DataAnnotations;

namespace Brbljator.Models;

// SCRUM-22: tablica Jezik
public class Jezik
{
    [Key]
    public int JezikID { get; set; }

    [Required, MaxLength(50)]
    public string Naziv { get; set; } = "";

    [Required, MaxLength(5)]
    public string Kod { get; set; } = "";     // npr. "en", "de"

    public List<Fraza> Fraze { get; set; } = new();
}
