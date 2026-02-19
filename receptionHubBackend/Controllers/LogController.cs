using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using receptionHubBackend.Data;
using receptionHubBackend.Models.DTOs;
using receptionHubBackend.Models.Enums;
using receptionHubBackend.Services;

namespace receptionHubBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")] // SAMO ADMIN MOZE VIDJETI LOGOVE
public class LogController : ControllerBase
{
    private readonly ReceptionHubDbContext _baza;
    private readonly ILogService _logService;

    public LogController(ReceptionHubDbContext baza, ILogService logService)
    {
        _baza = baza;
        _logService = logService;
    }

    // PREGLED SVIH LOGOVA
    [HttpGet]
    public async Task<ActionResult<object>> DajLogove(
        [FromQuery] int stranica = 1,
        [FromQuery] int poStranici = 50,
        [FromQuery] TipLoga? tip = null,
        [FromQuery] DateTime? od = null,
        [FromQuery] DateTime? doDatuma = null,
        [FromQuery] int? recepcionerId = null,
        [FromQuery] int? gostId = null,
        [FromQuery] int? statusKod = null)
    {
        try
        {
            var query = _baza.Logovi.AsQueryable();

            // FILTRIRANJ
            if (tip.HasValue)
                query = query.Where(l => l.Tip == tip.Value);

            if (od.HasValue)
                query = query.Where(l => l.Vrijeme >= od.Value);

            if (doDatuma.HasValue)
                query = query.Where(l => l.Vrijeme <= doDatuma.Value);

            if (recepcionerId.HasValue)
                query = query.Where(l => l.RecepcionerId == recepcionerId.Value);

            if (gostId.HasValue)
                query = query.Where(l => l.GostId == gostId.Value);

            if (statusKod.HasValue)
                query = query.Where(l => l.StatusKod == statusKod.Value);

            // UKUPAN BROJ LOGOVA
            var ukupno = await query.CountAsync();

            // SORTIRANJE
            var logovi = await query
                .OrderByDescending(l => l.Vrijeme) // Najnoviji prvi
                .Skip((stranica - 1) * poStranici)
                .Take(poStranici)
                .Select(l => new LogDto
                {
                    Id = l.Id,
                    Vrijeme = l.Vrijeme,
                    Tip = l.Tip,
                    Poruka = l.Poruka,
                    Detalji = l.Detalji,
                    Izvor = l.Izvor,
                    RecepcionerId = l.RecepcionerId,
                    GostId = l.GostId,
                    KorisnickoIme = l.KorisnickoIme,
                    HttpMetoda = l.HttpMetoda,
                    Putanja = l.Putanja,
                    StatusKod = l.StatusKod,
                    IPAdresa = l.IPAdresa,
                    TrajanjeMs = l.TrajanjeMs,
                    TipIzuzetka = l.TipIzuzetka
                })
                .ToListAsync();

            await _logService.LogInfoAsync(
                $"Admin pregledao logove (stranica {stranica}, {logovi.Count} zapisa)",
                "LogController"
            );

            return Ok(new
            {
                podaci = logovi,
                ukupno,
                stranica,
                poStranici,
                ukupnoStranica = (int)Math.Ceiling(ukupno / (double)poStranici)
            });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                "Greška pri dohvatanju logova",
                ex,
                "LogController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške pri dohvatanju logova" });
        }
    }

    // GET ODREDENI LOG
    [HttpGet("{id}")]
    public async Task<ActionResult<LogDto>> DajLog(int id)
    {
        try
        {
            var log = await _baza.Logovi.FindAsync(id);

            if (log == null)
            {
                await _logService.LogUpozorenjeAsync(
                    $"Pokušaj pristupa nepostojećem logu ID: {id}",
                    "LogController"
                );
                return NotFound(new { poruka = "Log nije pronađen" });
            }

            var logDto = new LogDto
            {
                Id = log.Id,
                Vrijeme = log.Vrijeme,
                Tip = log.Tip,
                Poruka = log.Poruka,
                Detalji = log.Detalji,
                Izvor = log.Izvor,
                RecepcionerId = log.RecepcionerId,
                GostId = log.GostId,
                KorisnickoIme = log.KorisnickoIme,
                HttpMetoda = log.HttpMetoda,
                Putanja = log.Putanja,
                StatusKod = log.StatusKod,
                IPAdresa = log.IPAdresa,
                UserAgent = log.UserAgent,
                Zaglavlja = log.Zaglavlja,
                TijeloZahtjeva = log.TijeloZahtjeva,
                TrajanjeMs = log.TrajanjeMs,
                TipIzuzetka = log.TipIzuzetka,
                StackTrace = log.StackTrace,
                TraceId = log.TraceId,
                SessionId = log.SessionId,
                DodatniPodaci = log.DodatniPodaci
            };

            return Ok(logDto);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                $"Greška pri dohvatanju loga ID: {id}",
                ex,
                "LogController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // LOGOVI ZA ODREDENOG RECEPCIONERA
    [HttpGet("recepcioner/{recepcionerId}")]
    public async Task<ActionResult<object>> LogoviPoRecepcioneru(
        int recepcionerId,
        [FromQuery] int stranica = 1,
        [FromQuery] int poStranici = 20)
    {
        try
        {
            var query = _baza.Logovi
                .Where(l => l.RecepcionerId == recepcionerId);

            var ukupno = await query.CountAsync();

            var logovi = await query
                .OrderByDescending(l => l.Vrijeme)
                .Skip((stranica - 1) * poStranici)
                .Take(poStranici)
                .Select(l => new LogDto
                {
                    Id = l.Id,
                    Vrijeme = l.Vrijeme,
                    Tip = l.Tip,
                    Poruka = l.Poruka,
                    Putanja = l.Putanja,
                    StatusKod = l.StatusKod,
                    IPAdresa = l.IPAdresa
                })
                .ToListAsync();

            return Ok(new
            {
                podaci = logovi,
                ukupno,
                recepcionerId
            });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync(
                $"Greška pri dohvatanju logova za recepcionera ID: {recepcionerId}",
                ex,
                "LogController"
            );
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

}