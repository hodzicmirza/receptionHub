using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptionHubBackend.Controllers.Dtos;
using receptionHubBackend.Data;
using receptionHubBackend.Models;
using receptionHubBackend.Models.Enums;
using receptionHubBackend.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace receptionHubBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GostController : ControllerBase
{
    private readonly ReceptionHubDbContext _baza;
    private readonly ILogService _logService;

    public GostController(ReceptionHubDbContext baza, ILogService logService)
    {
        _baza = baza;
        _logService = logService;
    }

    [HttpGet]
    public async Task<ActionResult<object>> DajGoste(
        [FromQuery] int stranica = 1,
        [FromQuery] int poStranici = 20,
        [FromQuery] string? sortirajPo = "vrijemeKreiranja",
        [FromQuery] string? smjer = "desc",
        [FromQuery] string? pretraga = null,
        [FromQuery] TipGosta? tipGosta = null,
        [FromQuery] bool? vip = null)
    {
        try
        {
            var query = _baza.Gosti.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                pretraga = pretraga.ToLower();
                query = query.Where(g =>
                    (g.Ime != null && g.Ime.ToLower().Contains(pretraga)) ||
                    (g.Prezime != null && g.Prezime.ToLower().Contains(pretraga)) ||
                    (g.NazivFirme != null && g.NazivFirme.ToLower().Contains(pretraga)) ||
                    (g.BrojTelefona != null && g.BrojTelefona.Contains(pretraga)) ||
                    (g.Email != null && g.Email.ToLower().Contains(pretraga)) ||
                    (g.Drzava != null && g.Drzava.ToLower().Contains(pretraga))
                );
            }

            if (tipGosta.HasValue)
                query = query.Where(g => g.TipGosta == tipGosta.Value);

            if (vip.HasValue)
                query = query.Where(g => g.VIPGost == vip.Value);

            query = sortirajPo?.ToLower() switch
            {
                "ime" => smjer == "asc" ? query.OrderBy(g => g.Ime) : query.OrderByDescending(g => g.Ime),
                "prezime" => smjer == "asc" ? query.OrderBy(g => g.Prezime) : query.OrderByDescending(g => g.Prezime),
                "drzava" => smjer == "asc" ? query.OrderBy(g => g.Drzava) : query.OrderByDescending(g => g.Drzava),
                "vip" => smjer == "asc" ? query.OrderBy(g => g.VIPGost) : query.OrderByDescending(g => g.VIPGost),
                _ => smjer == "asc" ? query.OrderBy(g => g.VrijemeKreiranja) : query.OrderByDescending(g => g.VrijemeKreiranja)
            };

            var ukupno = await query.CountAsync();

            var gosti = await query
                .Skip((stranica - 1) * poStranici)
                .Take(poStranici)
                .Select(g => new GostDto
                {
                    IdGosta = g.IdGosta,
                    Ime = g.Ime,
                    Prezime = g.Prezime,
                    TipGosta = g.TipGosta,
                    NazivFirme = g.NazivFirme,
                    KontaktOsoba = g.KontaktOsoba,
                    BrojTelefona = g.BrojTelefona,
                    Email = g.Email,
                    Drzava = g.Drzava,
                    TipDokumenta = g.TipDokumenta,
                    VIPGost = g.VIPGost,
                    Dodatno = g.Dodatno,
                    VrijemeKreiranja = g.VrijemeKreiranja,
                    RecepcionerId = g.RecepcionerId
                })
                .ToListAsync();

            await _logService.LogInfoAsync(
                $"Pregled gostiju (stranica {stranica}, {gosti.Count} zapisa, pretraga: '{pretraga ?? "bez"}')",
                "GostiController"
            );

            return Ok(new
            {
                podaci = gosti,
                ukupno,
                stranica,
                poStranici,
                ukupnoStranica = (int)Math.Ceiling(ukupno / (double)poStranici)
            });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dohvatanju gostiju", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri dohvatanju gostiju" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GostDto>> DajGosta(int id)
    {
        try
        {
            var gost = await _baza.Gosti
                .Include(g => g.RezervacijeGosta)
                    .ThenInclude(rg => rg.Rezervacija)
                .FirstOrDefaultAsync(g => g.IdGosta == id);

            if (gost == null)
            {
                await _logService.LogUpozorenjeAsync($"Pokušaj pristupa nepostojećem gostu ID: {id}", "GostiController");
                return NotFound(new { poruka = "Gost nije pronađen" });
            }

            var gostDto = new GostDto
            {
                IdGosta = gost.IdGosta,
                Ime = gost.Ime,
                Prezime = gost.Prezime,
                TipGosta = gost.TipGosta,
                NazivFirme = gost.NazivFirme,
                KontaktOsoba = gost.KontaktOsoba,
                BrojTelefona = gost.BrojTelefona,
                Email = gost.Email,
                Drzava = gost.Drzava,
                TipDokumenta = gost.TipDokumenta,
                SlikaDokumenta = gost.SlikaDokumenta,
                VIPGost = gost.VIPGost,
                Dodatno = gost.Dodatno,
                VrijemeKreiranja = gost.VrijemeKreiranja,
                RecepcionerId = gost.RecepcionerId,
                BrojRezervacija = gost.RezervacijeGosta?.Count ?? 0
            };

            await _logService.LogInfoAsync($"Pregled gosta ID: {id}", "GostiController");

            return Ok(gostDto);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri dohvatanju gosta ID: {id}", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpGet("pretraga")]
    public async Task<ActionResult<IEnumerable<GostDto>>> PretraziGoste(
        [FromQuery] string termin,
        [FromQuery] bool ukljuciArhivirane = false)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termin) || termin.Length < 2)
            {
                return BadRequest(new { poruka = "Termin za pretragu mora imati najmanje 2 karaktera" });
            }

            termin = termin.ToLower();
            var query = _baza.Gosti.AsQueryable();

            var gosti = await query
                .Where(g =>
                    (g.Ime != null && g.Ime.ToLower().Contains(termin)) ||
                    (g.Prezime != null && g.Prezime.ToLower().Contains(termin)) ||
                    (g.NazivFirme != null && g.NazivFirme.ToLower().Contains(termin)) ||
                    (g.BrojTelefona != null && g.BrojTelefona.Contains(termin)) ||
                    (g.Email != null && g.Email.ToLower().Contains(termin)) ||
                    (g.Drzava != null && g.Drzava.ToLower().Contains(termin)))
                .Select(g => new GostDto
                {
                    IdGosta = g.IdGosta,
                    Ime = g.Ime,
                    Prezime = g.Prezime,
                    TipGosta = g.TipGosta,
                    NazivFirme = g.NazivFirme,
                    BrojTelefona = g.BrojTelefona,
                    Drzava = g.Drzava,
                    VIPGost = g.VIPGost
                })
                .Take(50)
                .ToListAsync();

            await _logService.LogInfoAsync($"Pretraga gostiju: '{termin}' - pronađeno {gosti.Count} rezultata", "GostiController");

            return Ok(gosti);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri pretrazi gostiju", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri pretrazi" });
        }
    }

    [HttpGet("vip")]
    public async Task<ActionResult<IEnumerable<GostDto>>> DajVipGoste()
    {
        try
        {
            var vipGosti = await _baza.Gosti
                .Where(g => g.VIPGost)
                .Select(g => new GostDto
                {
                    IdGosta = g.IdGosta,
                    Ime = g.Ime,
                    Prezime = g.Prezime,
                    TipGosta = g.TipGosta,
                    NazivFirme = g.NazivFirme,
                    BrojTelefona = g.BrojTelefona,
                    Email = g.Email,
                    Drzava = g.Drzava,
                    Dodatno = g.Dodatno
                })
                .ToListAsync();

            return Ok(vipGosti);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dohvatanju VIP gostiju", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpGet("statistika")]
    public async Task<ActionResult<GostStatistikaDto>> StatistikaGostiju()
    {
        try
        {
            var danas = DateTime.UtcNow.Date;
            var pocetakMjeseca = new DateTime(danas.Year, danas.Month, 1);
            var topDrzave = await _baza.Gosti
            .Where(g => g.Drzava != null)
            .GroupBy(g => g.Drzava)
            .Select(g => new DrzavaStatistika
            {
                Drzava = g.Key!,
                Broj = g.Count()
            })
            .OrderByDescending(x => x.Broj)
            .Take(5)
            .ToListAsync();

            var statistika = new GostStatistikaDto()
            {
                Ukupno = await _baza.Gosti.CountAsync(),
                Danas = await _baza.Gosti.CountAsync(g => g.VrijemeKreiranja.Date == danas),
                OvajMjesec = await _baza.Gosti.CountAsync(g => g.VrijemeKreiranja >= pocetakMjeseca),
                FizickaLica = await _baza.Gosti.CountAsync(g => g.TipGosta == TipGosta.FizickoLice),
                PravnaLica = await _baza.Gosti.CountAsync(g => g.TipGosta == TipGosta.PravnoLice),
                VIP = await _baza.Gosti.CountAsync(g => g.VIPGost),
                TopDrzave = topDrzave
            };

            return Ok(statistika);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dohvatanju statistike gostiju", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<GostDto>> KreirajGosta([FromBody] KreirajGostaDto gostDto)
    {
        try
        {
            if (gostDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite sva obavezna polja", ModelState });
            }

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (gostDto.TipGosta == TipGosta.FizickoLice)
            {
                if (string.IsNullOrWhiteSpace(gostDto.Ime) || string.IsNullOrWhiteSpace(gostDto.Prezime))
                {
                    return BadRequest(new { poruka = "Za fizička lica ime i prezime su obavezni" });
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(gostDto.NazivFirme))
                {
                    return BadRequest(new { poruka = "Za pravna lica naziv firme je obavezan" });
                }
            }

            if (string.IsNullOrWhiteSpace(gostDto.BrojTelefona) && string.IsNullOrWhiteSpace(gostDto.Email))
            {
                return BadRequest(new { poruka = "Potrebno je unijeti barem broj telefona ili email" });
            }

            var noviGost = new Gost
            {
                TipGosta = gostDto.TipGosta,
                Ime = gostDto.Ime,
                Prezime = gostDto.Prezime,
                NazivFirme = gostDto.NazivFirme,
                KontaktOsoba = gostDto.KontaktOsoba,
                BrojTelefona = gostDto.BrojTelefona,
                Email = gostDto.Email,
                Drzava = gostDto.Drzava,
                TipDokumenta = gostDto.TipDokumenta,
                SlikaDokumenta = gostDto.SlikaDokumenta,
                VIPGost = gostDto.VIPGost,
                Dodatno = gostDto.Dodatno,
                RecepcionerId = trenutniKorisnikId
            };

            _baza.Gosti.Add(noviGost);
            await _baza.SaveChangesAsync();

            var resultDto = new GostDto
            {
                IdGosta = noviGost.IdGosta,
                Ime = noviGost.Ime,
                Prezime = noviGost.Prezime,
                TipGosta = noviGost.TipGosta,
                NazivFirme = noviGost.NazivFirme,
                BrojTelefona = noviGost.BrojTelefona,
                Email = noviGost.Email,
                Drzava = noviGost.Drzava,
                VIPGost = noviGost.VIPGost,
                VrijemeKreiranja = noviGost.VrijemeKreiranja
            };

            await _logService.LogAkcijuAsync(
                $"Kreiran novi gost: {(noviGost.TipGosta == TipGosta.FizickoLice ? $"{noviGost.Ime} {noviGost.Prezime}" : noviGost.NazivFirme)}",
                trenutniKorisnikId,
                noviGost.IdGosta
            );

            return CreatedAtAction(nameof(DajGosta), new { id = noviGost.IdGosta }, resultDto);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri kreiranju gosta", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri kreiranju gosta" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AzurirajGosta(int id, [FromBody] AzurirajGostaDto gostDto)
    {
        try
        {
            if (gostDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite ispravno sva polja", ModelState });
            }

            var gost = await _baza.Gosti.FindAsync(id);
            if (gost == null)
            {
                return NotFound(new { poruka = "Gost nije pronađen" });
            }

            if (!string.IsNullOrEmpty(gostDto.Email) && !new EmailAddressAttribute().IsValid(gostDto.Email))
            {
                return BadRequest(new { poruka = "Email nije ispravnog formata" });
            }

            gost.Ime = gostDto.Ime ?? gost.Ime;
            gost.Prezime = gostDto.Prezime ?? gost.Prezime;
            gost.NazivFirme = gostDto.NazivFirme ?? gost.NazivFirme;
            gost.KontaktOsoba = gostDto.KontaktOsoba ?? gost.KontaktOsoba;
            gost.BrojTelefona = gostDto.BrojTelefona ?? gost.BrojTelefona;
            gost.Email = gostDto.Email ?? gost.Email;
            gost.Drzava = gostDto.Drzava ?? gost.Drzava;
            gost.TipDokumenta = gostDto.TipDokumenta ?? gost.TipDokumenta;
            gost.SlikaDokumenta = gostDto.SlikaDokumenta ?? gost.SlikaDokumenta;
            gost.VIPGost = gostDto.VIPGost ?? gost.VIPGost;
            gost.Dodatno = gostDto.Dodatno ?? gost.Dodatno;
            gost.VrijemeAzuriranja = DateTime.UtcNow;

            await _baza.SaveChangesAsync();

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            await _logService.LogAkcijuAsync(
                $"Ažurirani podaci za gosta ID: {id}",
                trenutniKorisnikId,
                id
            );

            return Ok(new { poruka = "Podaci uspješno ažurirani" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri ažuriranju gosta ID: {id}", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpPatch("{id}/oznaciVip")]
    public async Task<IActionResult> OznaciVip(int id, [FromBody] PromijeniVipStatusDto vipStatus)
    {
        try
        {
            var gost = await _baza.Gosti.FindAsync(id);
            if (gost == null)
            {
                return NotFound(new { poruka = "Gost nije pronađen" });
            }

            gost.VIPGost = vipStatus.VipStatus;
            await _baza.SaveChangesAsync();

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            await _logService.LogAkcijuAsync(
                $"Gost ID: {id} {(vipStatus.VipStatus ? "označen kao VIP" : "uklonjen VIP status")}",
                trenutniKorisnikId,
                id
            );

            return Ok(new { poruka = $"Gost {(vipStatus.VipStatus ? "označen kao VIP" : "uklonjen VIP status")}" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri označavanju VIP gosta ID: {id}", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ObrisiGosta(int id)
    {
        try
        {
            var gost = await _baza.Gosti
                .Include(g => g.RezervacijeGosta)
                .FirstOrDefaultAsync(g => g.IdGosta == id);

            if (gost == null)
            {
                return NotFound(new { poruka = "Gost nije pronađen" });
            }

            if (gost.RezervacijeGosta != null && gost.RezervacijeGosta.Any())
            {
                return BadRequest(new { poruka = "Ne možete obrisati gosta koji ima aktivne rezervacije" });
            }

            _baza.Gosti.Remove(gost);
            await _baza.SaveChangesAsync();

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            await _logService.LogAkcijuAsync(
                $"Obrisan gost ID: {id}",
                trenutniKorisnikId,
                id
            );

            return Ok(new { poruka = "Gost uspješno obrisan" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri brisanju gosta ID: {id}", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpPost("{id}/arhiviraj")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ArhivirajGosta(int id)
    {
        try
        {
            var gost = await _baza.Gosti
                .Include(g => g.RezervacijeGosta)
                .FirstOrDefaultAsync(g => g.IdGosta == id);

            if (gost == null)
            {
                return NotFound(new { poruka = "Gost nije pronađen" });
            }

            if (gost.RezervacijeGosta != null && gost.RezervacijeGosta.Any())
            {
                return BadRequest(new { poruka = "Ne možete arhivirati gosta koji ima aktivne rezervacije" });
            }

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var arhivaService = HttpContext.RequestServices.GetRequiredService<IArhivaService>();

            var uspjeh = await arhivaService.ArhivirajGostaAsync(id, trenutniKorisnikId);

            if (!uspjeh)
                return BadRequest(new { poruka = "Arhiviranje nije uspjelo" });

            return Ok(new { poruka = "Gost uspješno arhiviran" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri arhiviranju gosta ID: {id}", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpGet("pdf/{id}")]
    public async Task<IActionResult> GenerisiPdfGosta(int id)
    {
        try
        {
            var gost = await _baza.Gosti
                .Include(g => g.RezervacijeGosta)
                .FirstOrDefaultAsync(g => g.IdGosta == id);

            if (gost == null)
            {
                return NotFound(new { poruka = "Gost nije pronađen" });
            }

            var pdfService = HttpContext.RequestServices.GetRequiredService<IPdfService>();
            var pdfBytes = await pdfService.GenerisiGostPdfAsync(gost);

            return File(pdfBytes, "application/pdf", $"gost_{id}_{DateTime.Now:yyyyMMdd}.pdf");
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri generisanju PDF-a za gosta ID: {id}", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri generisanju PDF-a" });
        }
    }

    [HttpGet("pdfLista")]
    public async Task<IActionResult> GenerisiPdfListu(
        [FromQuery] string? pretraga = null,
        [FromQuery] TipGosta? tipGosta = null)
    {
        try
        {
            var query = _baza.Gosti.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pretraga))
            {
                pretraga = pretraga.ToLower();
                query = query.Where(g =>
                    (g.Ime != null && g.Ime.ToLower().Contains(pretraga)) ||
                    (g.Prezime != null && g.Prezime.ToLower().Contains(pretraga)) ||
                    (g.NazivFirme != null && g.NazivFirme.ToLower().Contains(pretraga)) ||
                    (g.BrojTelefona != null && g.BrojTelefona.Contains(pretraga)));
            }

            if (tipGosta.HasValue)
                query = query.Where(g => g.TipGosta == tipGosta.Value);

            var gosti = await query.ToListAsync();
            var pdfService = HttpContext.RequestServices.GetRequiredService<IPdfService>();
            var pdfBytes = await pdfService.GenerisiListuGostijuPdfAsync(gosti);

            return File(pdfBytes, "application/pdf", $"gosti_lista_{DateTime.Now:yyyyMMdd}.pdf");
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri generisanju PDF liste gostiju", ex, "GostiController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri generisanju PDF-a" });
        }
    }
}