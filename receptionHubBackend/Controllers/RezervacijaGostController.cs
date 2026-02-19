using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptionHubBackend.Controllers.Dtos;
using receptionHubBackend.Data;
using receptionHubBackend.Models;
using receptionHubBackend.Models.Enums;
using receptionHubBackend.Services;

namespace receptionHubBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RezervacijaGostController : ControllerBase
{
    private readonly ReceptionHubDbContext _baza;
    private readonly ILogService _logService;

    public RezervacijaGostController(ReceptionHubDbContext baza, ILogService logService)
    {
        _baza = baza;
        _logService = logService;
    }

    // GET: api/rezervacijagost/rezervacija/{rezervacijaId} - Svi gosti u rezervaciji
    [HttpGet("rezervacija/{rezervacijaId}")]
    public async Task<ActionResult<IEnumerable<RezervacijaGostDto>>> DajGosteRezervacije(int rezervacijaId)
    {
        try
        {
            var rezervacija = await _baza.Rezervacije
                .FirstOrDefaultAsync(r => r.IdRezervacije == rezervacijaId);

            if (rezervacija == null)
            {
                return NotFound(new { poruka = "Rezervacija nije pronađena" });
            }

            var gosti = await _baza.RezervacijaGosti
                .Include(rg => rg.Gost)
                .Include(rg => rg.Rezervacija)
                .Where(rg => rg.RezervacijaId == rezervacijaId)
                .Select(rg => new RezervacijaGostDto
                {
                    Id = rg.Id,
                    RezervacijaId = rg.RezervacijaId,
                    GostId = rg.GostId,
                    ImeGosta = rg.Gost != null && rg.Gost.TipGosta == TipGosta.FizickoLice
                        ? $"{rg.Gost.Ime} {rg.Gost.Prezime}"
                        : null,
                    NazivFirme = rg.Gost != null && rg.Gost.TipGosta == TipGosta.PravnoLice
                        ? rg.Gost.NazivFirme
                        : null,
                    JeGlavniGost = rg.JeGlavniGost,
                    PosebneNapomene = rg.PosebneNapomene,
                    BrojRezervacije = rg.Rezervacija != null ? rg.Rezervacija.BrojRezervacije : null
                })
                .ToListAsync();

            return Ok(gosti);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri dohvatanju gostiju rezervacije ID: {rezervacijaId}", ex, "RezervacijaGostController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // GET: api/rezervacijagost/gost/{gostId} - Sve rezervacije gosta
    [HttpGet("gost/{gostId}")]
    public async Task<ActionResult<IEnumerable<RezervacijaGostDto>>> DajRezervacijeGosta(int gostId)
    {
        try
        {
            var gost = await _baza.Gosti.FindAsync(gostId);
            if (gost == null)
            {
                return NotFound(new { poruka = "Gost nije pronađen" });
            }

            var rezervacije = await _baza.RezervacijaGosti
                .Include(rg => rg.Rezervacija)
                .Include(rg => rg.Gost)
                .Where(rg => rg.GostId == gostId)
                .Select(rg => new RezervacijaGostDto
                {
                    Id = rg.Id,
                    RezervacijaId = rg.RezervacijaId,
                    GostId = rg.GostId,
                    ImeGosta = rg.Gost != null && rg.Gost.TipGosta == TipGosta.FizickoLice
                        ? $"{rg.Gost.Ime} {rg.Gost.Prezime}"
                        : null,
                    NazivFirme = rg.Gost != null && rg.Gost.TipGosta == TipGosta.PravnoLice
                        ? rg.Gost.NazivFirme
                        : null,
                    JeGlavniGost = rg.JeGlavniGost,
                    PosebneNapomene = rg.PosebneNapomene,
                    BrojRezervacije = rg.Rezervacija != null ? rg.Rezervacija.BrojRezervacije : null
                })
                .ToListAsync();

            return Ok(rezervacije);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri dohvatanju rezervacija za gosta ID: {gostId}", ex, "RezervacijaGostController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // GET: api/rezervacijagost/{id} - Pojedinačna veza
    [HttpGet("{id}")]
    public async Task<ActionResult<RezervacijaGostDto>> DajVeza(int id)
    {
        try
        {
            var veza = await _baza.RezervacijaGosti
                .Include(rg => rg.Gost)
                .Include(rg => rg.Rezervacija)
                .FirstOrDefaultAsync(rg => rg.Id == id);

            if (veza == null)
            {
                return NotFound(new { poruka = "Veza nije pronađena" });
            }

            var dto = new RezervacijaGostDto
            {
                Id = veza.Id,
                RezervacijaId = veza.RezervacijaId,
                GostId = veza.GostId,
                ImeGosta = veza.Gost != null && veza.Gost.TipGosta == TipGosta.FizickoLice
                    ? $"{veza.Gost.Ime} {veza.Gost.Prezime}"
                    : null,
                NazivFirme = veza.Gost != null && veza.Gost.TipGosta == TipGosta.PravnoLice
                    ? veza.Gost.NazivFirme
                    : null,
                JeGlavniGost = veza.JeGlavniGost,
                PosebneNapomene = veza.PosebneNapomene,
                BrojRezervacije = veza.Rezervacija?.BrojRezervacije
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri dohvatanju veze ID: {id}", ex, "RezervacijaGostController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // POST: api/rezervacijagost - Dodaj gosta u rezervaciju
    [HttpPost]
    public async Task<ActionResult<RezervacijaGostDto>> DodajGostaURezervaciju([FromBody] KreirajRezervacijaGostDto kreirajDto)
    {
        try
        {
            if (kreirajDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite sva obavezna polja", ModelState });
            }

            // Provjeri da li rezervacija postoji
            var rezervacija = await _baza.Rezervacije.FindAsync(kreirajDto.RezervacijaId);
            if (rezervacija == null)
            {
                return NotFound(new { poruka = "Rezervacija nije pronađena" });
            }

            // Provjeri da li gost postoji
            var gost = await _baza.Gosti.FindAsync(kreirajDto.GostId);
            if (gost == null)
            {
                return NotFound(new { poruka = "Gost nije pronađen" });
            }

            // Provjeri da li je gost već dodan u ovu rezervaciju
            var postoji = await _baza.RezervacijaGosti
                .AnyAsync(rg => rg.RezervacijaId == kreirajDto.RezervacijaId && rg.GostId == kreirajDto.GostId);

            if (postoji)
            {
                return BadRequest(new { poruka = "Ovaj gost je već dodan u rezervaciju" });
            }

            // Ako je glavni gost, provjeri da li već postoji glavni gost
            if (kreirajDto.JeGlavniGost)
            {
                var imaGlavnog = await _baza.RezervacijaGosti
                    .AnyAsync(rg => rg.RezervacijaId == kreirajDto.RezervacijaId && rg.JeGlavniGost);

                if (imaGlavnog)
                {
                    return BadRequest(new { poruka = "Ova rezervacija već ima glavnog gosta" });
                }
            }

            var novaVeza = new RezervacijaGost
            {
                RezervacijaId = kreirajDto.RezervacijaId,
                GostId = kreirajDto.GostId,
                JeGlavniGost = kreirajDto.JeGlavniGost,
                PosebneNapomene = kreirajDto.PosebneNapomene
            };

            _baza.RezervacijaGosti.Add(novaVeza);
            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Gost ID: {kreirajDto.GostId} dodan u rezervaciju ID: {kreirajDto.RezervacijaId}",
                0,  // Ovde možeš dodati ID trenutnog korisnika
                kreirajDto.GostId
            );

            return CreatedAtAction(nameof(DajVeza), new { id = novaVeza.Id }, new RezervacijaGostDto
            {
                Id = novaVeza.Id,
                RezervacijaId = novaVeza.RezervacijaId,
                GostId = novaVeza.GostId,
                JeGlavniGost = novaVeza.JeGlavniGost,
                PosebneNapomene = novaVeza.PosebneNapomene
            });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri dodavanju gosta u rezervaciju", ex, "RezervacijaGostController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // PUT: api/rezervacijagost/{id} - Ažuriraj vezu
    [HttpPut("{id}")]
    public async Task<IActionResult> AzurirajVeza(int id, [FromBody] AzurirajRezervacijaGostDto azurirajDto)
    {
        try
        {
            if (azurirajDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite ispravno sva polja", ModelState });
            }

            var veza = await _baza.RezervacijaGosti.FindAsync(id);
            if (veza == null)
            {
                return NotFound(new { poruka = "Veza nije pronađena" });
            }

            // Ažuriraj polja
            if (azurirajDto.JeGlavniGost.HasValue)
            {
                // Ako postavlja novog glavnog gosta, provjeri da li već postoji drugi glavni
                if (azurirajDto.JeGlavniGost.Value && !veza.JeGlavniGost)
                {
                    var imaGlavnog = await _baza.RezervacijaGosti
                        .AnyAsync(rg => rg.RezervacijaId == veza.RezervacijaId
                                   && rg.JeGlavniGost
                                   && rg.Id != id);

                    if (imaGlavnog)
                    {
                        return BadRequest(new { poruka = "Ova rezervacija već ima glavnog gosta" });
                    }
                }
                veza.JeGlavniGost = azurirajDto.JeGlavniGost.Value;
            }

            if (azurirajDto.PosebneNapomene != null)
            {
                veza.PosebneNapomene = azurirajDto.PosebneNapomene;
            }

            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Ažurirana veza ID: {id}",
                0,  // Ovde možeš dodati ID trenutnog korisnika
                veza.GostId
            );

            return Ok(new { poruka = "Veza uspješno ažurirana" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri ažuriranju veze ID: {id}", ex, "RezervacijaGostController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // PATCH: api/rezervacijagost/{id}/glavni-gost - Postavi kao glavnog gosta
    [HttpPatch("{id}/glavni-gost")]
    public async Task<IActionResult> PostaviGlavnogGosta(int id)
    {
        try
        {
            var veza = await _baza.RezervacijaGosti.FindAsync(id);
            if (veza == null)
            {
                return NotFound(new { poruka = "Veza nije pronađena" });
            }

            // Ukloni glavnog gosta ako postoji
            var trenutniGlavni = await _baza.RezervacijaGosti
                .FirstOrDefaultAsync(rg => rg.RezervacijaId == veza.RezervacijaId && rg.JeGlavniGost);

            if (trenutniGlavni != null)
            {
                trenutniGlavni.JeGlavniGost = false;
            }

            veza.JeGlavniGost = true;
            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Gost ID: {veza.GostId} postavljen kao glavni za rezervaciju ID: {veza.RezervacijaId}",
                0,
                veza.GostId
            );

            return Ok(new { poruka = "Glavni gost uspješno postavljen" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri postavljanju glavnog gosta za vezu ID: {id}", ex, "RezervacijaGostController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // DELETE: api/rezervacijagost/{id} - Ukloni gosta iz rezervacije
    [HttpDelete("{id}")]
    public async Task<IActionResult> UkloniGostaIzRezervacije(int id)
    {
        try
        {
            var veza = await _baza.RezervacijaGosti.FindAsync(id);
            if (veza == null)
            {
                return NotFound(new { poruka = "Veza nije pronađena" });
            }

            _baza.RezervacijaGosti.Remove(veza);
            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Gost ID: {veza.GostId} uklonjen iz rezervacije ID: {veza.RezervacijaId}",
                0,
                veza.GostId
            );

            return Ok(new { poruka = "Gost uspješno uklonjen iz rezervacije" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync($"Greška pri uklanjanju gosta iz rezervacije za vezu ID: {id}", ex, "RezervacijaGostController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }
}