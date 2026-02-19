using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptionHubBackend.Controllers.Dtos;
using receptionHubBackend.Data;
using receptionHubBackend.Models;
using receptionHubBackend.Models.Enums;
using receptionHubBackend.Services;
using System.Security.Claims;

namespace receptionHubBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RezervacijaController : ControllerBase
{
    private readonly ReceptionHubDbContext _baza;
    private readonly ILogService _logService;

    public RezervacijaController(ReceptionHubDbContext baza, ILogService logService)
    {
        _baza = baza;
        _logService = logService;
    }

    [HttpGet]
    public async Task<ActionResult<object>> DajRezervacije(
        [FromQuery] int stranica = 1,
        [FromQuery] int poStranici = 20,
        [FromQuery] DateTime? od = null,
        [FromQuery] DateTime? @do = null,
        [FromQuery] StatusRezervacije? status = null,
        [FromQuery] int? sobaId = null,
        [FromQuery] int? gostId = null)
    {
        try
        {
            var query = _baza.Rezervacije
                .Include(r => r.Soba)
                .Include(r => r.GostiURezervaciji)
                    .ThenInclude(rg => rg.Gost)
                .AsQueryable();

            if (od.HasValue)
                query = query.Where(r => r.DatumDolaska >= od.Value);

            if (@do.HasValue)
                query = query.Where(r => r.DatumOdlaska <= @do.Value);

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            if (sobaId.HasValue)
                query = query.Where(r => r.SobaId == sobaId.Value);

            if (gostId.HasValue)
                query = query.Where(r => r.GostiURezervaciji.Any(rg => rg.GostId == gostId.Value));

            var ukupno = await query.CountAsync();

            var rezervacije = await query
                .OrderByDescending(r => r.DatumDolaska)
                .Skip((stranica - 1) * poStranici)
                .Take(poStranici)
                .Select(r => new KratkiPregledRezervacijeDto
                {
                    IdRezervacije = r.IdRezervacije,
                    BrojRezervacije = r.BrojRezervacije,
                    BrojSobe = r.Soba != null ? r.Soba.BrojSobe : null,
                    DatumDolaska = r.DatumDolaska,
                    DatumOdlaska = r.DatumOdlaska,
                    BrojNocenja = r.BrojNocenja,
                    UkupnoGostiju = r.UkupnoGostiju,
                    UkupnaCijena = r.UkupnaCijena,
                    Status = r.Status,
                    ImeGlavnogGosta = r.GostiURezervaciji
                        .Where(rg => rg.JeGlavniGost)
                        .Select(rg => rg.Gost != null && rg.Gost.TipGosta == TipGosta.FizickoLice
                            ? rg.Gost.Ime + " " + rg.Gost.Prezime
                            : rg.Gost != null ? rg.Gost.NazivFirme : null)
                        .FirstOrDefault()
                })
                .ToListAsync();

            await _logService.LogInfoAsync($"Pregled rezervacija (stranica {stranica}, {rezervacije.Count} rezervacija)", "RezervacijaController");

            return Ok(new
            {
                podaci = rezervacije,
                ukupno,
                stranica,
                poStranici,
                ukupnoStranica = (int)Math.Ceiling(ukupno / (double)poStranici)
            });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dohvatanju rezervacija", ex, "RezervacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri dohvatanju rezervacija" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RezervacijaDto>> DajRezervaciju(int id)
    {
        try
        {
            var rezervacija = await _baza.Rezervacije
                .Include(r => r.Soba)
                .Include(r => r.Recepcioner)
                .Include(r => r.GostiURezervaciji)
                    .ThenInclude(rg => rg.Gost)
                .FirstOrDefaultAsync(r => r.IdRezervacije == id);

            if (rezervacija == null)
            {
                await _logService.LogUpozorenjeAsync($"Pokušaj pristupa nepostojećoj rezervaciji ID: {id}", "RezervacijaController");
                return NotFound(new { poruka = "Rezervacija nije pronađena" });
            }

            var gosti = rezervacija.GostiURezervaciji?
                .Select(rg => new RezervacijaGostDto
                {
                    Id = rg.Id,
                    GostId = rg.GostId,
                    ImeGosta = rg.Gost != null && rg.Gost.TipGosta == TipGosta.FizickoLice
                        ? $"{rg.Gost.Ime} {rg.Gost.Prezime}"
                        : null,
                    NazivFirme = rg.Gost != null && rg.Gost.TipGosta == TipGosta.PravnoLice
                        ? rg.Gost.NazivFirme
                        : null,
                    JeGlavniGost = rg.JeGlavniGost,
                    PosebneNapomene = rg.PosebneNapomene
                })
                .ToList() ?? new List<RezervacijaGostDto>();

            var dto = new RezervacijaDto
            {
                IdRezervacije = rezervacija.IdRezervacije,
                BrojRezervacije = rezervacija.BrojRezervacije,
                SobaId = rezervacija.SobaId,
                BrojSobe = rezervacija.Soba?.BrojSobe,
                RecepcionerId = rezervacija.RecepcionerId,
                ImeRecepcionera = rezervacija.Recepcioner != null
                    ? $"{rezervacija.Recepcioner.Ime} {rezervacija.Recepcioner.Prezime}"
                    : null,
                DatumDolaska = rezervacija.DatumDolaska,
                DatumOdlaska = rezervacija.DatumOdlaska,
                BrojNocenja = rezervacija.BrojNocenja,
                BrojOdraslih = rezervacija.BrojOdraslih,
                BrojDjece = rezervacija.BrojDjece,
                UkupnoGostiju = rezervacija.UkupnoGostiju,
                CijenaPoNoci = rezervacija.CijenaPoNoci,
                Popust = rezervacija.Popust,
                UkupnaCijena = rezervacija.UkupnaCijena,
                NacinRezervacije = rezervacija.NacinRezervacije,
                EksterniBrojRezervacije = rezervacija.EksterniBrojRezervacije,
                Status = rezervacija.Status,
                Zahtjevi = rezervacija.Zahtjevi,
                Napomena = rezervacija.Napomena,
                VrijemeKreiranja = rezervacija.VrijemeKreiranja,
                Gosti = gosti
            };

            await _logService.LogInfoAsync($"Pregled rezervacije ID: {id}", "RezervacijaController");

            return Ok(dto);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri dohvatanju rezervacije ID: {id}", ex, "RezervacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpGet("dostupnost")]
    public async Task<IActionResult> ProvjeriDostupnost([FromQuery] ProvjeraDostupnostiDto dto)
    {
        try
        {
            if (dto.DatumOdlaska <= dto.DatumDolaska)
            {
                return BadRequest(new { poruka = "Datum odlaska mora biti nakon datuma dolaska" });
            }

            var soba = await _baza.Sobe.FindAsync(dto.SobaId);
            if (soba == null)
            {
                return NotFound(new { poruka = "Soba nije pronađena" });
            }

            // Provjeri da li postoji rezervacija za taj period
            var postojiRezervacija = await _baza.Rezervacije
                .AnyAsync(r => r.SobaId == dto.SobaId &&
                            r.Status != StatusRezervacije.Otkazana &&
                            r.DatumDolaska < dto.DatumOdlaska &&
                            r.DatumOdlaska > dto.DatumDolaska);

            return Ok(new
            {
                dostupna = !postojiRezervacija,
                sobaBroj = soba.BrojSobe,
                od = dto.DatumDolaska.ToString("dd.MM.yyyy"),
                @do = dto.DatumOdlaska.ToString("dd.MM.yyyy")
            });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri provjeri dostupnosti", ex, "RezervacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<RezervacijaDto>> KreirajRezervaciju([FromBody] KreirajRezervacijuDto kreirajDto)
    {
        try
        {
            if (kreirajDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite sva obavezna polja", ModelState });
            }

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // Provjeri da li soba postoji
            var soba = await _baza.Sobe.FindAsync(kreirajDto.SobaId);
            if (soba == null)
            {
                return NotFound(new { poruka = "Soba nije pronađena" });
            }

            // Provjeri dostupnost sobe
            var postojiRezervacija = await _baza.Rezervacije
                .AnyAsync(r => r.SobaId == kreirajDto.SobaId &&
                            r.Status != StatusRezervacije.Otkazana &&
                            r.DatumDolaska < kreirajDto.DatumOdlaska &&
                            r.DatumOdlaska > kreirajDto.DatumDolaska);

            if (postojiRezervacija)
            {
                return BadRequest(new { poruka = "Soba nije dostupna za odabrani period" });
            }

            // Generiši broj rezervacije
            var brojRezervacije = $"R-{DateTime.Now:yyyyMMdd}-{DateTime.Now.Ticks % 10000}";

            var novaRezervacija = new Rezervacija
            {
                BrojRezervacije = brojRezervacije,
                SobaId = kreirajDto.SobaId,
                RecepcionerId = trenutniKorisnikId,
                DatumDolaska = kreirajDto.DatumDolaska,
                DatumOdlaska = kreirajDto.DatumOdlaska,
                BrojOdraslih = kreirajDto.BrojOdraslih,
                BrojDjece = kreirajDto.BrojDjece,
                CijenaPoNoci = kreirajDto.CijenaPoNoci,
                Popust = kreirajDto.Popust,
                UkupnaCijena = (kreirajDto.CijenaPoNoci * (kreirajDto.DatumOdlaska - kreirajDto.DatumDolaska).Days)
                                - (kreirajDto.Popust ?? 0),
                NacinRezervacije = kreirajDto.NacinRezervacije,
                EksterniBrojRezervacije = kreirajDto.EksterniBrojRezervacije,
                Zahtjevi = kreirajDto.Zahtjevi,
                Napomena = kreirajDto.Napomena,
                VrijemeKreiranja = DateTime.UtcNow
            };

            _baza.Rezervacije.Add(novaRezervacija);
            await _baza.SaveChangesAsync();

            // Dodaj goste u rezervaciju ako su poslati
            if (kreirajDto.GostIds != null && kreirajDto.GostIds.Any())
            {
                foreach (var gostId in kreirajDto.GostIds)
                {
                    var veza = new RezervacijaGost
                    {
                        RezervacijaId = novaRezervacija.IdRezervacije,
                        GostId = gostId,
                        JeGlavniGost = kreirajDto.GlavniGostId == gostId
                    };
                    _baza.RezervacijaGosti.Add(veza);
                }
                await _baza.SaveChangesAsync();
            }

            await _logService.LogAkcijuAsync(
                $"Kreirana nova rezervacija: {brojRezervacije}",
                trenutniKorisnikId,
                null
            );

            return CreatedAtAction(nameof(DajRezervaciju), new { id = novaRezervacija.IdRezervacije },
                await DajRezervaciju(novaRezervacija.IdRezervacije));
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri kreiranju rezervacije", ex, "RezervacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri kreiranju rezervacije" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AzurirajRezervaciju(int id, [FromBody] AzurirajRezervacijuDto azurirajDto)
    {
        try
        {
            if (azurirajDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite ispravno sva polja", ModelState });
            }

            var rezervacija = await _baza.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound(new { poruka = "Rezervacija nije pronađena" });
            }

            // Ažuriraj polja
            if (azurirajDto.SobaId.HasValue)
                rezervacija.SobaId = azurirajDto.SobaId.Value;

            if (azurirajDto.DatumDolaska.HasValue)
                rezervacija.DatumDolaska = azurirajDto.DatumDolaska.Value;

            if (azurirajDto.DatumOdlaska.HasValue)
                rezervacija.DatumOdlaska = azurirajDto.DatumOdlaska.Value;

            if (azurirajDto.BrojOdraslih.HasValue)
                rezervacija.BrojOdraslih = azurirajDto.BrojOdraslih.Value;

            if (azurirajDto.BrojDjece.HasValue)
                rezervacija.BrojDjece = azurirajDto.BrojDjece.Value;

            if (azurirajDto.CijenaPoNoci.HasValue)
                rezervacija.CijenaPoNoci = azurirajDto.CijenaPoNoci.Value;

            if (azurirajDto.Popust.HasValue)
                rezervacija.Popust = azurirajDto.Popust.Value;

            if (azurirajDto.NacinRezervacije.HasValue)
                rezervacija.NacinRezervacije = azurirajDto.NacinRezervacije.Value;

            if (azurirajDto.Status.HasValue)
                rezervacija.Status = azurirajDto.Status.Value;

            if (!string.IsNullOrWhiteSpace(azurirajDto.EksterniBrojRezervacije))
                rezervacija.EksterniBrojRezervacije = azurirajDto.EksterniBrojRezervacije;

            if (!string.IsNullOrWhiteSpace(azurirajDto.Zahtjevi))
                rezervacija.Zahtjevi = azurirajDto.Zahtjevi;

            if (!string.IsNullOrWhiteSpace(azurirajDto.Napomena))
                rezervacija.Napomena = azurirajDto.Napomena;

            // Preračunaj ukupnu cijenu
            rezervacija.UkupnaCijena = (rezervacija.CijenaPoNoci * (rezervacija.DatumOdlaska - rezervacija.DatumDolaska).Days)
                                        - (rezervacija.Popust ?? 0);

            rezervacija.VrijemeAzuriranja = DateTime.UtcNow;

            await _baza.SaveChangesAsync();

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            await _logService.LogAkcijuAsync(
                $"Ažurirana rezervacija ID: {id}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = "Rezervacija uspješno ažurirana" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri ažuriranju rezervacije ID: {id}", ex, "RezervacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> PromijeniStatus(int id, [FromBody] StatusRezervacijeDto statusDto)
    {
        try
        {
            if (statusDto == null)
            {
                return BadRequest(new { poruka = "Status je obavezan" });
            }

            var rezervacija = await _baza.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound(new { poruka = "Rezervacija nije pronađena" });
            }

            var stariStatus = rezervacija.Status;
            rezervacija.Status = statusDto.Status;

            if (statusDto.Status == StatusRezervacije.Otkazana && !string.IsNullOrWhiteSpace(statusDto.Razlog))
            {
                rezervacija.VrijemeOtkazivanja = DateTime.UtcNow;
                rezervacija.RazlogOtkazivanja = statusDto.Razlog;
            }

            rezervacija.VrijemeAzuriranja = DateTime.UtcNow;

            await _baza.SaveChangesAsync();

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            await _logService.LogAkcijuAsync(
                $"Promijenjen status rezervacije {rezervacija.BrojRezervacije}: {stariStatus} -> {statusDto.Status}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = $"Status promijenjen u {statusDto.Status}" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri promjeni statusa rezervacije ID: {id}", ex, "RezervacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpPost("{id}/otkazi")]
    public async Task<IActionResult> OtkaziRezervaciju(int id, [FromBody] OtkaziRezervacijuDto otkaziDto)
    {
        try
        {
            if (otkaziDto == null || string.IsNullOrWhiteSpace(otkaziDto.Razlog))
            {
                return BadRequest(new { poruka = "Razlog otkazivanja je obavezan" });
            }

            var rezervacija = await _baza.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound(new { poruka = "Rezervacija nije pronađena" });
            }

            if (rezervacija.Status == StatusRezervacije.Otkazana)
            {
                return BadRequest(new { poruka = "Rezervacija je već otkazana" });
            }

            rezervacija.Status = StatusRezervacije.Otkazana;
            rezervacija.VrijemeOtkazivanja = DateTime.UtcNow;
            rezervacija.RazlogOtkazivanja = otkaziDto.Razlog;
            rezervacija.VrijemeAzuriranja = DateTime.UtcNow;

            await _baza.SaveChangesAsync();

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            await _logService.LogAkcijuAsync(
                $"Otkazana rezervacija {rezervacija.BrojRezervacije}: {otkaziDto.Razlog}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = "Rezervacija uspješno otkazana" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri otkazivanju rezervacije ID: {id}", ex, "RezervacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ObrisiRezervaciju(int id)
    {
        try
        {
            var rezervacija = await _baza.Rezervacije
                .Include(r => r.GostiURezervaciji)
                .FirstOrDefaultAsync(r => r.IdRezervacije == id);

            if (rezervacija == null)
            {
                return NotFound(new { poruka = "Rezervacija nije pronađena" });
            }

            if (rezervacija.Status != StatusRezervacije.Otkazana)
            {
                return BadRequest(new { poruka = "Možete obrisati samo otkazane rezervacije" });
            }

            // Prvo obriši sve veze sa gostima
            if (rezervacija.GostiURezervaciji != null && rezervacija.GostiURezervaciji.Any())
            {
                _baza.RezervacijaGosti.RemoveRange(rezervacija.GostiURezervaciji);
            }

            _baza.Rezervacije.Remove(rezervacija);
            await _baza.SaveChangesAsync();

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            await _logService.LogAkcijuAsync(
                $"Obrisana rezervacija ID: {id}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = "Rezervacija uspješno obrisana" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri brisanju rezervacije ID: {id}", ex, "RezervacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }
}