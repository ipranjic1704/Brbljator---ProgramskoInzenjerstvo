# Brbljator

Aplikacija za učenje jezika – ASP.NET Core Web API (.NET 10), Entity Framework Core, SQLite.

## Pokretanje
1. `git clone` repozitorija
2. Otvori `Brbljator.slnx` u Visual Studiju 2022/2026 (potreban .NET 10 SDK)
3. F5 – baza `brbljator.db` kreira se automatski pri prvom pokretanju (tablice Korisnik, Jezik, Fraza + početni podaci)

Bazu ispočetka: zaustavi aplikaciju, obriši `Brbljator/brbljator.db*` i pokreni ponovno.

## Testni korisnici
| Korisničko ime | Lozinka | Uloga |
|---|---|---|
| admin | admin123 | administrator |
| test | test123 | korisnik |

## Rad s Gitom
Grana `feature/SCRUM-xx-opis` → Pull Request → review kolege → merge u `master`.
