using System.Security.Claims;
using Brbljator.Data;
using Brbljator.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Brbljator.Controllers;

public record LoginZahtjev(string? KorisnickoIme, string? Lozinka);

[ApiController]
[Route("api/auth")]
public class AuthController(BrbljatorDbContext db) : ControllerBase
{
    // SCRUM-14: POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginZahtjev zahtjev)
    {
        if (string.IsNullOrWhiteSpace(zahtjev.KorisnickoIme) || string.IsNullOrEmpty(zahtjev.Lozinka))
            return BadRequest(new { poruka = "Unesite korisničko ime i lozinku." });

        var ime = zahtjev.KorisnickoIme.Trim();
        var korisnik = await db.Korisnici.FirstOrDefaultAsync(k => k.KorisnickoIme == ime || k.Email == ime);

        if (korisnik is null)
            return Unauthorized(new { poruka = Korisnik.PorukaPogresnaPrijava });

        if (!korisnik.Login(zahtjev.Lozinka, out var poruka))
            return Unauthorized(new { poruka });

        // Prijava uspjela – izdaj kolačić (cookie) s podacima o korisniku
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, korisnik.IDUser.ToString()),
            new(ClaimTypes.Name, korisnik.KorisnickoIme)
        };
        if (korisnik.IsAdmin)
            claims.Add(new(ClaimTypes.Role, "Admin"));

        var identitet = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identitet));

        return Ok(new { id = korisnik.IDUser, korisnickoIme = korisnik.KorisnickoIme, isAdmin = korisnik.IsAdmin });
    }

    // POST /api/auth/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    // GET /api/auth/ja – tko je trenutno prijavljen
    [Authorize]
    [HttpGet("ja")]
    public IActionResult Ja() =>
        Ok(new { korisnickoIme = User.Identity!.Name, isAdmin = User.IsInRole("Admin") });
}
