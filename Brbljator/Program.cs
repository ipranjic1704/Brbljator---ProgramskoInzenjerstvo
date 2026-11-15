using Brbljator.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// SQLite baza – datoteka brbljator.db u mapi projekta
builder.Services.AddDbContext<BrbljatorDbContext>(o =>
    o.UseSqlite(builder.Configuration.GetConnectionString("Brbljator")));

var app = builder.Build();

// Kreiraj bazu i tablice ako ne postoje + dodaj testne korisnike
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BrbljatorDbContext>();
    db.Database.EnsureCreated();
    PocetniPodaci.Dodaj(db);
}

app.UseAuthorization();

app.MapControllers();

app.Run();
