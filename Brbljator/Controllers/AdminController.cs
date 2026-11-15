using System.Security.Claims;
using Brbljator.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brbljator.Controllers;

// SCRUM-16 / SCRUM-45: samo korisnici s ulogom Admin (IsAdmin = true) smiju ovamo.
// Neprijavljeni dobiju 401, prijavljeni ne-admini 403.
[ApiController]
[Route("api/admin/korisnici")]
[Authorize(Roles = "Admin")]
public class AdminController(BrbljatorDbContext db) : ControllerBase
{
    // SCRUM-44: GET /api/admin/korisnici – popis svih korisnika
    [HttpGet]
    public async Task<IActionResult> Popis()
    {
        var korisnici = await db.Korisnici
            .OrderBy(k => k.KorisnickoIme)
            .Select(k => new
            {
                id = k.IDUser,
                korisnickoIme = k.KorisnickoIme,
                email = k.Email,
                isAdmin = k.IsAdmin,
                blokiran = k.Blokiran
            })
            .ToListAsync();

        return Ok(korisnici);
    }

    // SCRUM-16: PUT /api/admin/korisnici/{id}/blokiran   tijelo: true ili false
    [HttpPut("{id:int}/blokiran")]
    public async Task<IActionResult> PostaviBlokiran(int id, [FromBody] bool blokiran)
    {
        var mojId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (id == mojId)
            return BadRequest(new { poruka = "Ne možete blokirati vlastiti račun." });

        var korisnik = await db.Korisnici.FindAsync(id);
        if (korisnik is null)
            return NotFound(new { poruka = "Korisnik ne postoji." });

        korisnik.Blokiran = blokiran;
        await db.SaveChangesAsync();
        return NoContent();
    }
}
