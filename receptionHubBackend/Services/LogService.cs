using receptionHubBackend.Data;
using receptionHubBackend.Models;
using receptionHubBackend.Models.Enums;
using System.Security.Claims;

namespace receptionHubBackend.Services
{
    public class LogService : ILogService
    {
        private readonly ReceptionHubDbContext _baza;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(ReceptionHubDbContext baza, IHttpContextAccessor httpContextAccessor)
        {
            _baza = baza;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogInfoAsync(string poruka, string? izvor = null)
        {
            await LogAsync(TipLoga.Info, poruka, null, null, izvor);
        }

        public async Task LogUpozorenjeAsync(string poruka, string? izvor = null)
        {
            await LogAsync(TipLoga.Upozorenje, poruka, null, null, izvor);
        }

        public async Task LogGreskuAsync(string poruka, Exception? ex = null, string? izvor = null)
        {
            await LogAsync(TipLoga.Greska, poruka, ex?.Message, ex?.StackTrace, izvor);
        }

        public async Task LogAkcijuAsync(string poruka, int? recepcionerId = null, int? gostId = null)
        {
            await LogAsync(TipLoga.Audit, poruka, null, null, "Korisnička akcija", recepcionerId, gostId);
        }

        public async Task LogHttpZahtjevAsync(string metoda, string putanja, int statusKod, long trajanjeMs)
        {
            var poruka = $"{metoda} {putanja} - {statusKod} ({trajanjeMs}ms)";

            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = httpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

            var log = new Log
            {
                Vrijeme = DateTime.UtcNow,
                Tip = statusKod >= 400 ? TipLoga.Upozorenje : TipLoga.Info,
                Poruka = poruka,
                HttpMetoda = metoda,
                Putanja = putanja,
                StatusKod = statusKod,
                TrajanjeMs = trajanjeMs,
                IPAdresa = httpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = httpContext?.Request?.Headers["User-Agent"].ToString(),
                KorisnickoIme = httpContext?.User?.Identity?.Name,
                Izvor = "HTTP"
            };

            if (int.TryParse(userId, out int id))
            {
                if (userRole == "Recepcioner" || userRole == "Admin")
                    log.RecepcionerId = id;
                else
                    log.GostId = id;
            }

            _baza.Logovi.Add(log);
            await _baza.SaveChangesAsync();
        }

        private async Task LogAsync(
            TipLoga tip,
            string poruka,
            string? detalji = null,
            string? stackTrace = null,
            string? izvor = null,
            int? recepcionerId = null,
            int? gostId = null)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var log = new Log
            {
                Vrijeme = DateTime.UtcNow,
                Tip = tip,
                Poruka = poruka,
                Detalji = detalji,
                StackTrace = stackTrace,
                Izvor = izvor,
                RecepcionerId = recepcionerId,
                GostId = gostId,
                IPAdresa = httpContext?.Connection?.RemoteIpAddress?.ToString(),
                UserAgent = httpContext?.Request?.Headers["User-Agent"].ToString(),
                Putanja = httpContext?.Request?.Path,
                HttpMetoda = httpContext?.Request?.Method,
                TraceId = System.Diagnostics.Activity.Current?.Id
            };

            _baza.Logovi.Add(log);
            await _baza.SaveChangesAsync();
        }
    }
}