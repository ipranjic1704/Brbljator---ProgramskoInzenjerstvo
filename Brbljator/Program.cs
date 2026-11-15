using Brbljator.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// SQLite baza – datoteka brbljator.db u mapi projekta
builder.Services.AddDbContext<BrbljatorDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Brbljator")));

// SCRUM-14: prijava preko kolačića (cookie)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.Cookie.Name = "Brbljator.Prijava";
        o.Cookie.HttpOnly = true;                  // JavaScript ne može pročitati kolačić
        o.Cookie.SameSite = SameSiteMode.Strict;
        o.ExpireTimeSpan = TimeSpan.FromHours(8);

        // API vraća 401/403 umjesto preusmjeravanja na stranicu za prijavu
        o.Events.OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        o.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Kreiraj bazu i tablice ako ne postoje + dodaj testne korisnike
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BrbljatorDbContext>();
    db.Database.EnsureCreated();
    PocetniPodaci.Dodaj(db);
}

app.UseDefaultFiles();   // "/" otvara wwwroot/index.html
app.UseStaticFiles();    // HTML/CSS iz mape wwwroot

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
