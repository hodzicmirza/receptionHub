
namespace receptionHubBackend.Services
{
    public interface ILogService
    {
        Task LogInfoAsync(string poruka, string? izvor = null);
        Task LogUpozorenjeAsync(string poruka, string? izvor = null);
        Task LogGreskuAsync(string poruka, Exception? ex = null, string? izvor = null);
        Task LogAkcijuAsync(string poruka, int? recepcionerId = null, int? gostId = null);
        Task LogHttpZahtjevAsync(string metoda, string putanja, int statusKod, long trajanjeMs);
    }
}