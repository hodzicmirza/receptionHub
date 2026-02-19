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
public class SobaController : ControllerBase
{
    private readonly ReceptionHubDbContext _baza;
    private readonly ILogService _logService;

    public SobaController(ReceptionHubDbContext baza, ILogService logService)
    {
        _baza = baza;
        _logService = logService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SobaDto>>> DajSobe()
    {
        try
        {
            var sobe = await _baza.Sobe
                .OrderBy(s => s.BrojSobe)
                .Select(s => new SobaDto
                {
                    IdSobe = s.IdSobe,
                    BrojSobe = s.BrojSobe,
                    TipSobe = s.TipSobe,
                    MaksimalnoGostiju = s.MaksimalnoGostiju,
                    BrojKreveta = s.BrojKreveta,
                    BrojBracnihKreveta = s.BrojBracnihKreveta,
                    BrojOdvojenihKreveta = s.BrojOdvojenihKreveta,
                    ImaDodatniKrevet = s.ImaDodatniKrevet,
                    CijenaPoNociBAM = s.CijenaPoNociBAM,
                    Opis = s.Opis,
                    KratkiOpis = s.KratkiOpis,
                    ImaTv = s.ImaTv,
                    ImaKlimu = s.ImaKlimu,
                    ImaMiniBar = s.ImaMiniBar,
                    ImaPogledNaGrad = s.ImaPogledNaGrad,
                    ImaWiFi = s.ImaWiFi,
                    ImaRadniSto = s.ImaRadniSto,
                    ImaFen = s.ImaFen,
                    ImaPeglu = s.ImaPeglu,
                    ImaKupatilo = s.ImaKupatilo,
                    ImaTus = s.ImaTus,
                    Status = s.Status,
                    PlaniranoOslobadjanje = s.PlaniranoOslobadjanje,
                    GlavnaSlika = s.GlavnaSlika,
                    Napomena = s.Napomena,
                    VrijemeKreiranja = s.VrijemeKreiranja,
                    BrojAktivnihRezervacija = s.Rezervacije != null 
                        ? s.Rezervacije.Count(r => r.Status == StatusRezervacije.Potvrdjena || r.Status == StatusRezervacije.Prijavljena)
                        : 0
                })
                .ToListAsync();

            await _logService.LogInfoAsync($"Pregled svih soba ({sobe.Count} soba)", "SobaController");

            return Ok(sobe);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dohvatanju soba", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri dohvatanju soba" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SobaDto>> DajSobu(int id)
    {
        try
        {
            var soba = await _baza.Sobe
                .Include(s => s.Rezervacije)
                .FirstOrDefaultAsync(s => s.IdSobe == id);

            if (soba == null)
            {
                await _logService.LogUpozorenjeAsync($"Pokušaj pristupa nepostojećoj sobi ID: {id}", "SobaController");
                return NotFound(new { poruka = "Soba nije pronađena" });
            }

            var sobaDto = new SobaDto
            {
                IdSobe = soba.IdSobe,
                BrojSobe = soba.BrojSobe,
                TipSobe = soba.TipSobe,
                MaksimalnoGostiju = soba.MaksimalnoGostiju,
                BrojKreveta = soba.BrojKreveta,
                BrojBracnihKreveta = soba.BrojBracnihKreveta,
                BrojOdvojenihKreveta = soba.BrojOdvojenihKreveta,
                ImaDodatniKrevet = soba.ImaDodatniKrevet,
                CijenaPoNociBAM = soba.CijenaPoNociBAM,
                Opis = soba.Opis,
                KratkiOpis = soba.KratkiOpis,
                ImaTv = soba.ImaTv,
                ImaKlimu = soba.ImaKlimu,
                ImaMiniBar = soba.ImaMiniBar,
                ImaPogledNaGrad = soba.ImaPogledNaGrad,
                ImaWiFi = soba.ImaWiFi,
                ImaRadniSto = soba.ImaRadniSto,
                ImaFen = soba.ImaFen,
                ImaPeglu = soba.ImaPeglu,
                ImaKupatilo = soba.ImaKupatilo,
                ImaTus = soba.ImaTus,
                Status = soba.Status,
                PlaniranoOslobadjanje = soba.PlaniranoOslobadjanje,
                GlavnaSlika = soba.GlavnaSlika,
                Napomena = soba.Napomena,
                VrijemeKreiranja = soba.VrijemeKreiranja,
                KreiraoRecepcionerId = soba.KreiraoRecepcionerId,
                VrijemeAzuriranja = soba.VrijemeAzuriranja,
                BrojAktivnihRezervacija = soba.Rezervacije != null
                    ? soba.Rezervacije.Count(r => r.Status == StatusRezervacije.Potvrdjena || r.Status == StatusRezervacije.Prijavljena)
                    : 0
            };

            await _logService.LogInfoAsync($"Pregled sobe ID: {id}", "SobaController");

            return Ok(sobaDto);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri dohvatanju sobe ID: {id}", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpGet("broj/{brojSobe}")]
    public async Task<ActionResult<SobaDto>> DajSobuPoBroju(string brojSobe)
    {
        try
        {
            var soba = await _baza.Sobe
                .Include(s => s.Rezervacije)
                .FirstOrDefaultAsync(s => s.BrojSobe == brojSobe);

            if (soba == null)
            {
                return NotFound(new { poruka = $"Soba broj {brojSobe} nije pronađena" });
            }

            var sobaDto = new SobaDto
            {
                IdSobe = soba.IdSobe,
                BrojSobe = soba.BrojSobe,
                TipSobe = soba.TipSobe,
                MaksimalnoGostiju = soba.MaksimalnoGostiju,
                BrojKreveta = soba.BrojKreveta,
                CijenaPoNociBAM = soba.CijenaPoNociBAM,
                Status = soba.Status,
                GlavnaSlika = soba.GlavnaSlika,
                BrojAktivnihRezervacija = soba.Rezervacije != null
                    ? soba.Rezervacije.Count(r => r.Status == StatusRezervacije.Potvrdjena || r.Status == StatusRezervacije.Prijavljena)
                    : 0
            };

            return Ok(sobaDto);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri dohvatanju sobe broj: {brojSobe}", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<SobaDto>>> DajSobePoStatusu(StatusSobe status)
    {
        try
        {
            var sobe = await _baza.Sobe
                .Where(s => s.Status == status)
                .OrderBy(s => s.BrojSobe)
                .Select(s => new SobaDto
                {
                    IdSobe = s.IdSobe,
                    BrojSobe = s.BrojSobe,
                    TipSobe = s.TipSobe,
                    MaksimalnoGostiju = s.MaksimalnoGostiju,
                    BrojKreveta = s.BrojKreveta,
                    CijenaPoNociBAM = s.CijenaPoNociBAM,
                    Status = s.Status,
                    GlavnaSlika = s.GlavnaSlika
                })
                .ToListAsync();

            await _logService.LogInfoAsync($"Pregled soba sa statusom {status}: {sobe.Count} soba", "SobaController");

            return Ok(sobe);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dohvatanju soba po statusu", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpGet("tip/{tip}")]
    public async Task<ActionResult<IEnumerable<SobaDto>>> DajSobePoTipu(TipSobe tip)
    {
        try
        {
            var sobe = await _baza.Sobe
                .Where(s => s.TipSobe == tip)
                .OrderBy(s => s.BrojSobe)
                .Select(s => new SobaDto
                {
                    IdSobe = s.IdSobe,
                    BrojSobe = s.BrojSobe,
                    TipSobe = s.TipSobe,
                    MaksimalnoGostiju = s.MaksimalnoGostiju,
                    CijenaPoNociBAM = s.CijenaPoNociBAM,
                    Status = s.Status
                })
                .ToListAsync();

            return Ok(sobe);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dohvatanju soba po tipu", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpGet("slobodne")]
    public async Task<ActionResult<IEnumerable<SobaDto>>> DajSlobodneSobe()
    {
        try
        {
            var sobe = await _baza.Sobe
                .Where(s => s.Status == StatusSobe.Slobodna)
                .OrderBy(s => s.BrojSobe)
                .Select(s => new SobaDto
                {
                    IdSobe = s.IdSobe,
                    BrojSobe = s.BrojSobe,
                    TipSobe = s.TipSobe,
                    MaksimalnoGostiju = s.MaksimalnoGostiju,
                    CijenaPoNociBAM = s.CijenaPoNociBAM,
                    Status = s.Status,
                    GlavnaSlika = s.GlavnaSlika
                })
                .ToListAsync();

            return Ok(sobe);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dohvatanju slobodnih soba", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SobaDto>> KreirajSobu([FromBody] KreirajSobuDto kreirajSobuDto)
    {
        try
        {
            if (kreirajSobuDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite sva obavezna polja", ModelState });
            }

            // Provjeri da li već postoji soba sa istim brojem
            var postoji = await _baza.Sobe
                .AnyAsync(s => s.BrojSobe == kreirajSobuDto.BrojSobe);

            if (postoji)
            {
                return BadRequest(new { poruka = $"Soba sa brojem {kreirajSobuDto.BrojSobe} već postoji" });
            }

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var novaSoba = new Soba
            {
                BrojSobe = kreirajSobuDto.BrojSobe,
                TipSobe = kreirajSobuDto.TipSobe,
                MaksimalnoGostiju = kreirajSobuDto.MaksimalnoGostiju,
                BrojKreveta = kreirajSobuDto.BrojKreveta,
                BrojBracnihKreveta = kreirajSobuDto.BrojBracnihKreveta,
                BrojOdvojenihKreveta = kreirajSobuDto.BrojOdvojenihKreveta,
                ImaDodatniKrevet = kreirajSobuDto.ImaDodatniKrevet,
                CijenaPoNociBAM = kreirajSobuDto.CijenaPoNociBAM,
                Opis = kreirajSobuDto.Opis,
                KratkiOpis = kreirajSobuDto.KratkiOpis,
                ImaTv = kreirajSobuDto.ImaTv,
                ImaKlimu = kreirajSobuDto.ImaKlimu,
                ImaMiniBar = kreirajSobuDto.ImaMiniBar,
                ImaPogledNaGrad = kreirajSobuDto.ImaPogledNaGrad,
                ImaWiFi = kreirajSobuDto.ImaWiFi,
                ImaRadniSto = kreirajSobuDto.ImaRadniSto,
                ImaFen = kreirajSobuDto.ImaFen,
                ImaPeglu = kreirajSobuDto.ImaPeglu,
                ImaKupatilo = kreirajSobuDto.ImaKupatilo,
                ImaTus = kreirajSobuDto.ImaTus,
                Status = StatusSobe.Slobodna,
                GlavnaSlika = kreirajSobuDto.GlavnaSlika,
                Napomena = kreirajSobuDto.Napomena,
                VrijemeKreiranja = DateTime.UtcNow,
                KreiraoRecepcionerId = trenutniKorisnikId
            };

            _baza.Sobe.Add(novaSoba);
            await _baza.SaveChangesAsync();

            var sobaDto = new SobaDto
            {
                IdSobe = novaSoba.IdSobe,
                BrojSobe = novaSoba.BrojSobe,
                TipSobe = novaSoba.TipSobe,
                MaksimalnoGostiju = novaSoba.MaksimalnoGostiju,
                CijenaPoNociBAM = novaSoba.CijenaPoNociBAM,
                Status = novaSoba.Status
            };

            await _logService.LogAkcijuAsync(
                $"Kreirana nova soba: {novaSoba.BrojSobe}",
                trenutniKorisnikId,
                null
            );

            return CreatedAtAction(nameof(DajSobu), new { id = novaSoba.IdSobe }, sobaDto);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri kreiranju sobe", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške pri kreiranju sobe" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AzurirajSobu(int id, [FromBody] AzurirajSobuDto azurirajSobuDto)
    {
        try
        {
            if (azurirajSobuDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite ispravno sva polja", ModelState });
            }

            var sobaIzBaze = await _baza.Sobe.FindAsync(id);
            if (sobaIzBaze == null)
            {
                return NotFound(new { poruka = "Soba nije pronađena" });
            }

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            // AZURIRAJ STA JE POSLATO
            if (!string.IsNullOrWhiteSpace(azurirajSobuDto.BrojSobe))
                sobaIzBaze.BrojSobe = azurirajSobuDto.BrojSobe;

            if (azurirajSobuDto.TipSobe.HasValue)
                sobaIzBaze.TipSobe = azurirajSobuDto.TipSobe.Value;

            if (azurirajSobuDto.MaksimalnoGostiju.HasValue)
                sobaIzBaze.MaksimalnoGostiju = azurirajSobuDto.MaksimalnoGostiju.Value;

            if (azurirajSobuDto.BrojKreveta.HasValue)
                sobaIzBaze.BrojKreveta = azurirajSobuDto.BrojKreveta.Value;

            if (azurirajSobuDto.BrojBracnihKreveta.HasValue)
                sobaIzBaze.BrojBracnihKreveta = azurirajSobuDto.BrojBracnihKreveta.Value;

            if (azurirajSobuDto.BrojOdvojenihKreveta.HasValue)
                sobaIzBaze.BrojOdvojenihKreveta = azurirajSobuDto.BrojOdvojenihKreveta.Value;

            if (azurirajSobuDto.ImaDodatniKrevet.HasValue)
                sobaIzBaze.ImaDodatniKrevet = azurirajSobuDto.ImaDodatniKrevet.Value;

            if (azurirajSobuDto.CijenaPoNociBAM.HasValue)
                sobaIzBaze.CijenaPoNociBAM = azurirajSobuDto.CijenaPoNociBAM.Value;

            sobaIzBaze.Opis = azurirajSobuDto.Opis ?? sobaIzBaze.Opis;
            sobaIzBaze.KratkiOpis = azurirajSobuDto.KratkiOpis ?? sobaIzBaze.KratkiOpis;

            if (azurirajSobuDto.ImaTv.HasValue)
                sobaIzBaze.ImaTv = azurirajSobuDto.ImaTv.Value;

            if (azurirajSobuDto.ImaKlimu.HasValue)
                sobaIzBaze.ImaKlimu = azurirajSobuDto.ImaKlimu.Value;

            if (azurirajSobuDto.ImaMiniBar.HasValue)
                sobaIzBaze.ImaMiniBar = azurirajSobuDto.ImaMiniBar.Value;

            if (azurirajSobuDto.ImaPogledNaGrad.HasValue)
                sobaIzBaze.ImaPogledNaGrad = azurirajSobuDto.ImaPogledNaGrad.Value;

            if (azurirajSobuDto.ImaWiFi.HasValue)
                sobaIzBaze.ImaWiFi = azurirajSobuDto.ImaWiFi.Value;

            if (azurirajSobuDto.ImaRadniSto.HasValue)
                sobaIzBaze.ImaRadniSto = azurirajSobuDto.ImaRadniSto.Value;

            if (azurirajSobuDto.ImaFen.HasValue)
                sobaIzBaze.ImaFen = azurirajSobuDto.ImaFen.Value;

            if (azurirajSobuDto.ImaPeglu.HasValue)
                sobaIzBaze.ImaPeglu = azurirajSobuDto.ImaPeglu.Value;

            if (azurirajSobuDto.ImaKupatilo.HasValue)
                sobaIzBaze.ImaKupatilo = azurirajSobuDto.ImaKupatilo.Value;

            if (azurirajSobuDto.ImaTus.HasValue)
                sobaIzBaze.ImaTus = azurirajSobuDto.ImaTus.Value;

            if (azurirajSobuDto.Status.HasValue)
                sobaIzBaze.Status = azurirajSobuDto.Status.Value;

            if (azurirajSobuDto.PlaniranoOslobadjanje.HasValue)
                sobaIzBaze.PlaniranoOslobadjanje = azurirajSobuDto.PlaniranoOslobadjanje.Value;

            sobaIzBaze.GlavnaSlika = azurirajSobuDto.GlavnaSlika ?? sobaIzBaze.GlavnaSlika;
            sobaIzBaze.Napomena = azurirajSobuDto.Napomena ?? sobaIzBaze.Napomena;
            sobaIzBaze.VrijemeAzuriranja = DateTime.UtcNow;
            sobaIzBaze.AzuriraoRecepcionerId = trenutniKorisnikId;

            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Ažurirana soba: {sobaIzBaze.BrojSobe}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = "Soba uspješno ažurirana" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri ažuriranju sobe ID: {id}", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> PromijeniStatus(int id, [FromBody] StatusSobeDto statusSobeDto)
    {
        try
        {
            if (statusSobeDto == null)
            {
                return BadRequest(new { poruka = "Status je obavezan" });
            }

            var soba = await _baza.Sobe.FindAsync(id);
            if (soba == null)
            {
                return NotFound(new { poruka = "Soba nije pronađena" });
            }

            var stariStatus = soba.Status;
            soba.Status = statusSobeDto.Status;
            soba.PlaniranoOslobadjanje = statusSobeDto.PlaniranoOslobadjanje;
            soba.VrijemeAzuriranja = DateTime.UtcNow;

            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Promijenjen status sobe {soba.BrojSobe}: {stariStatus} -> {statusSobeDto.Status}",
                soba.AzuriraoRecepcionerId ?? 0,
                null
            );

            return Ok(new { poruka = $"Status promijenjen u {statusSobeDto.Status}" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri promjeni statusa sobe ID: {id}", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ObrisiSobu(int id)
    {
        try
        {
            var soba = await _baza.Sobe
                .Include(s => s.Rezervacije)
                .FirstOrDefaultAsync(s => s.IdSobe == id);

            if (soba == null)
            {
                return NotFound(new { poruka = "Soba nije pronađena" });
            }

            // IMA LI AKTIVNE REZERVACIJE
            if (soba.Rezervacije != null && soba.Rezervacije.Any(r => 
                r.Status == StatusRezervacije.Potvrdjena || r.Status == StatusRezervacije.Prijavljena))
            {
                return BadRequest(new { poruka = "Ne možete obrisati sobu koja ima aktivne rezervacije" });
            }

            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            _baza.Sobe.Remove(soba);
            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Obrisana soba: {soba.BrojSobe}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = "Soba uspješno obrisana" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri brisanju sobe ID: {id}", ex, "SobaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }
}