using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using receptionHubBackend.Controllers.Dtos;
using receptionHubBackend.Services;

namespace receptionHubBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutentifikacijaController : ControllerBase
{
    private readonly IAuthenticationService _authService;
    private readonly ILogService _logService;
    public AutentifikacijaController(IAuthenticationService authService, ILogService logService)
    {
        _authService = authService;
        _logService = logService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (loginDto == null || !ModelState.IsValid)
            {
                await _logService.LogUpozorenjeAsync($"Neuspješna validacija login forme za korisnika: {loginDto?.KorisnickoIme ?? "nepoznato"}", "AutentifikacijaController");
                return BadRequest(new { poruka = "Popunite tražena polja" });
            }

            await _logService.LogInfoAsync($"Pokušaj logina za korisničko ime: {loginDto?.KorisnickoIme}", "AutentifikacijaController");
            var token = await _authService.AutentifikujAsync(loginDto!);

            if (token == null)
            {
                await _logService.LogUpozorenjeAsync($"Neuspješan login - pogrešni kredencijali za: {loginDto?.KorisnickoIme}", "AutentifikacijaController");
                return Unauthorized(new { poruka = "Pogrešno korisničko ime ili lozinka" });
            }

            await _logService.LogAkcijuAsync($"Uspješan login: {token.Ime} {token.Prezime}",
                int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0"),
                null
            );
            return Ok(token);
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška prilikom logina", ex, "AutentifikacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške na serveru", detalji = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _logService.LogAkcijuAsync($"Korisnik se odjavio", int.TryParse(userId, out int id) ? id : 0, null);

            return Ok(new { poruka = "Uspješno odjavljeni" });
        }
        catch (Exception ex)
        {
            await _logService.LogGreskuAsync("Greška pri odjavi", ex, "AutentifikacijaController");
            return StatusCode(500, new { poruka = "Došlo je do greške" });
        }
    }

    // TODO POKUŠATI IMPLEMENTIRATI REFRESH TOKEN DA SE SVAKIH 15 MINUTA TOKEN REFRESHA RADI SIGURNOSNIH RAZLOGA
}
