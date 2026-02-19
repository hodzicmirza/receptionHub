using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptionHubBackend.Controllers.Dtos;
using receptionHubBackend.Data;
using receptionHubBackend.Models;
using receptionHubBackend.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace receptionHubBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // SAMO AUTORIZOVANI KORISNICI MOGU PRISTUPITI (PREKO JWT)
public class RecepcionerController : ControllerBase
{
    private readonly ReceptionHubDbContext _baza;
    private readonly ILogService _logService;
    private readonly IPasswordHasher<Recepcioner> _passwordHasher;

    public RecepcionerController(
        ReceptionHubDbContext baza,
        ILogService logService,
        IPasswordHasher<Recepcioner> passwordHasher)
    {
        _baza = baza;
        _logService = logService;
        _passwordHasher = passwordHasher;
    }

    // GET SVE RECEPCIONERE - SAMO ADMIN
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<RecepcionerDto>>> DajRecepcionere()
    {
        try
        {
            var recepcioneri = await _baza.Recepcioneri
                .ToListAsync();

            await _logService.LogInfoAsync(
                $"Admin pregledao listu recepcionera ({recepcioneri.Count} korisnika)", "RecepcionerController");

            return Ok(recepcioneri);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                "Greška pri dohvatanju recepcionera", ex, "RecepcionerController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške pri dohvatanju recepcionera" });
        }
    }

    // GET ODREDENOG RECEPCIONERA MOZE ADMIN ILI SAMI RECEPCIONER
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Recepcioner")]
    public async Task<ActionResult<RecepcionerDto>> DajRecepcionera(int id)
    {
        try
        {
            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var trenutnaPozicija = User.FindFirst(ClaimTypes.Role)?.Value;

            if (trenutnaPozicija != "Admin" && trenutniKorisnikId != id)
            {
                await _logService.LogUpozorenjeAsync($"Pokušaj pristupa tuđem profilu: ID {id}",
                "RecepcionerController"
                );
                return Forbid();
            }

            var recepcioner = await _baza.Recepcioneri.FindAsync(id);
            if (recepcioner == null)
            {
                await _logService.LogUpozorenjeAsync(
                    $"Pokušaj pristupa nepostojećem recepcioneru ID: {id}",
                    "RecepcionerController"
                );
                return NotFound(new { poruka = "Recepcioner nije pronađen" });
            }

            var vratiRecepcionera = new RecepcionerDto
            {
                IdRecepcionera = recepcioner.IdRecepcionera,
                Ime = recepcioner.Ime,
                Prezime = recepcioner.Prezime,
                KorisnickoIme = recepcioner.KorisnickoIme,
                Email = recepcioner.Email,
                BrojTelefona = recepcioner.BrojTelefona,
                Pozicija = recepcioner.Pozicija,
                Aktivan = recepcioner.Aktivan,
                DatumKreiranjaRacuna = recepcioner.DatumKreiranjaRacuna,
                PosljednjiLogin = recepcioner.PosljednjiLogin,
                SlikaProfila = recepcioner.SlikaProfila,
                Napomena = recepcioner.Napomena
            };

            await _logService.LogInfoAsync(
                $"Pregled profila: {recepcioner.KorisnickoIme}", "RecepcionerController");

            return Ok(vratiRecepcionera);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                $"Greška pri dohvatanju recepcionera ID: {id}", ex,
                "RecepcionerController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške sa bazom" });
        }
    }

    // KREIRAJ RECEPCIONERA - SAMO ADMIN MOZE
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<RecepcionerDto>> KreirajRecepcionera([FromBody] KreirajRecepcioneraDto recepcionerDto)
    {
        try
        {
            if (recepcionerDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite sva obavezna polja", ModelState });
            }

            // Provjera da li korisničko ime već postoji
            var postoji = await _baza.Recepcioneri
                .AnyAsync(r => r.KorisnickoIme == recepcionerDto.KorisnickoIme);

            if (postoji)
            {
                await _logService.LogUpozorenjeAsync(
                    $"Pokušaj kreiranja duplog korisničkog imena: {recepcionerDto.KorisnickoIme}",
                    "RecepcionerController"
                );
                return BadRequest(new { poruka = "Korisničko ime već postoji" });
            }

            // Provjera emaila
            postoji = await _baza.Recepcioneri
                .AnyAsync(r => r.Email == recepcionerDto.Email);

            if (postoji)
            {
                await _logService.LogUpozorenjeAsync(
                    $"Pokušaj kreiranja duplog emaila: {recepcionerDto.Email}",
                    "RecepcionerController"
                );
                return BadRequest(new { poruka = "Email već postoji" });
            }

            var noviRecepcioner = new Recepcioner
            {
                Ime = recepcionerDto.Ime,
                Prezime = recepcionerDto.Prezime,
                KorisnickoIme = recepcionerDto.KorisnickoIme,
                Email = recepcionerDto.Email,
                BrojTelefona = recepcionerDto.BrojTelefona,
                Pozicija = recepcionerDto.Pozicija,
                Aktivan = true,
                LozinkaHash = _passwordHasher.HashPassword(new Recepcioner(), recepcionerDto.Lozinka),
                SlikaProfila = recepcionerDto.SlikaProfila,
                Napomena = recepcionerDto.Napomena
            };

            _baza.Recepcioneri.Add(noviRecepcioner);
            await _baza.SaveChangesAsync();

            var resultRecepcioner = new RecepcionerDto
            {
                IdRecepcionera = noviRecepcioner.IdRecepcionera,
                Ime = noviRecepcioner.Ime,
                Prezime = noviRecepcioner.Prezime,
                KorisnickoIme = noviRecepcioner.KorisnickoIme,
                Email = noviRecepcioner.Email,
                BrojTelefona = noviRecepcioner.BrojTelefona,
                Pozicija = noviRecepcioner.Pozicija,
                Aktivan = noviRecepcioner.Aktivan,
                DatumKreiranjaRacuna = noviRecepcioner.DatumKreiranjaRacuna
            };

            await _logService.LogAkcijuAsync(
                $"Kreiran novi recepcioner: {noviRecepcioner.KorisnickoIme} ({noviRecepcioner.Pozicija})",
                int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
                null
            );

            return CreatedAtAction(nameof(DajRecepcionera), new { id = noviRecepcioner.IdRecepcionera }, resultRecepcioner);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                "Greška pri kreiranju recepcionera", ex,
                "RecepcionerController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške pri kreiranju" });
        }
    }

    // PUT: IZMJENI RECEPCIONERA SAMO SVOJ ILI ADMIN SVE
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Recepcioner")]
    public async Task<IActionResult> UpdateRecepcioner(int id, [FromBody] UpdateRecepcioneraDto updateRecepcioneraDto)
    {
        try
        {
            if (updateRecepcioneraDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite ispravno sva polja", ModelState });
            }

            var recepcioner = await _baza.Recepcioneri.FindAsync(id);
            if (recepcioner == null)
            {
                return NotFound(new { poruka = "Recepcioner nije pronađen" });
            }

            // AUTORIZACIJA
            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var trenutnaPozicija = User.FindFirst(ClaimTypes.Role)?.Value;

            if (trenutnaPozicija != "Admin" && trenutniKorisnikId != id)
            {
                return Forbid();
            }

            if (!string.IsNullOrEmpty(updateRecepcioneraDto.Email) &&
                !new EmailAddressAttribute().IsValid(updateRecepcioneraDto.Email))
            {
                return BadRequest(new { poruka = "Email nije ispravnog formata" });
            }

            // UPDATE PODATAKA
            recepcioner.Ime = updateRecepcioneraDto.Ime ?? recepcioner.Ime;
            recepcioner.Prezime = updateRecepcioneraDto.Prezime ?? recepcioner.Prezime;
            recepcioner.Email = updateRecepcioneraDto.Email ?? recepcioner.Email;
            recepcioner.BrojTelefona = updateRecepcioneraDto.BrojTelefona ?? recepcioner.BrojTelefona;
            recepcioner.SlikaProfila = updateRecepcioneraDto.SlikaProfila ?? recepcioner.SlikaProfila;
            recepcioner.Napomena = updateRecepcioneraDto.Napomena ?? recepcioner.Napomena;

            // SAMO ADMIN MOZE MJENJATI POZICIJU I STATUS
            if (trenutnaPozicija == "Admin")
            {
                if (updateRecepcioneraDto.Pozicija.HasValue)
                    recepcioner.Pozicija = updateRecepcioneraDto.Pozicija.Value;

                if (updateRecepcioneraDto.Aktivan.HasValue)
                    recepcioner.Aktivan = updateRecepcioneraDto.Aktivan.Value;
            }

            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Ažurirani podaci za: {recepcioner.KorisnickoIme}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = "Podaci uspješno ažurirani" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                $"Greška pri ažuriranju recepcionera ID: {id}",
                ex,
                "RecepcionerController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    //  ZASEBNO MJENJANJE LOZINKE
    [HttpPatch("{id}/promijeniLozinku")]
    [Authorize(Roles = "Admin,Recepcioner")]
    public async Task<IActionResult> PromijeniLozinku(int id, [FromBody] PromjeniLozinkuDto novaLozinkaDto)
    {
        try
        {
            if (novaLozinkaDto == null || !ModelState.IsValid)
            {
                return BadRequest(new { poruka = "Popunite sva polja", ModelState });
            }

            var recepcioner = await _baza.Recepcioneri.FindAsync(id);
            if (recepcioner == null)
            {
                return NotFound(new { poruka = "Recepcioner nije pronađen" });
            }

            // AUTORIZACIJA
            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var trenutnaRola = User.FindFirst(ClaimTypes.Role)?.Value;

            if (trenutnaRola != "Admin" && trenutniKorisnikId != id)
            {
                return Forbid();
            }

            // ADMIN MOZE IZMJENITI LOZNKU IAKO JE NE ZNA
            if (trenutnaRola != "Admin")
            {
                var result = _passwordHasher.VerifyHashedPassword(
                    recepcioner,
                    recepcioner.LozinkaHash,
                    novaLozinkaDto.StaraLozinka
                );

                if (result == PasswordVerificationResult.Failed)
                {
                    await _logService.LogUpozorenjeAsync(
                        $"Pogrešna stara lozinka za korisnika: {recepcioner.KorisnickoIme}, ID: ${recepcioner.IdRecepcionera}",
                        "RecepcionerController"
                    );
                    return BadRequest(new { poruka = "Pogrešna stara lozinka" });
                }
            }

            // POSTAVLJANJE NOVE LOZINKE
            recepcioner.LozinkaHash = _passwordHasher.HashPassword(recepcioner, novaLozinkaDto.NovaLozinka);

            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Promijenjena lozinka za korisnika: {recepcioner.KorisnickoIme}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = "Lozinka uspješno promijenjena" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                $"Greška pri promjeni lozinke za ID: {id}",
                ex,
                "RecepcionerController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // DELETE RECEPCIONERA - SAMO ADMIN
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRecepcioner(int id)
    {
        try
        {
            var recepcioner = await _baza.Recepcioneri.FindAsync(id);
            if (recepcioner == null)
            {
                return NotFound(new { poruka = "Recepcioner nije pronađen" });
            }

            // NE MOZEMO OBRISATI SAMOG SEBE (ADMIN SEBE NE MOZE IZBRISATI)
            var trenutniKorisnikId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (trenutniKorisnikId == id)
            {
                return BadRequest(new { poruka = "Ne možete obrisati sami sebe" });
            }

            _baza.Recepcioneri.Remove(recepcioner);
            await _baza.SaveChangesAsync();

            await _logService.LogAkcijuAsync(
                $"Obrisan recepcioner: {recepcioner.KorisnickoIme}",
                trenutniKorisnikId,
                null
            );

            return Ok(new { poruka = "Recepcioner uspješno obrisan" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                $"Greška pri brisanju recepcionera ID: {id}",
                ex,
                "RecepcionerController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }
}