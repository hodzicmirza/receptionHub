using receptionHubBackend.Models;

namespace receptionHubBackend.Services;

public interface IPdfService
{
    Task<byte[]> GenerisiGostPdfAsync(Gost gost);
    Task<byte[]> GenerisiListuGostijuPdfAsync(List<Gost> gosti);
    Task<byte[]> GenerisiRezervacijuPdfAsync(Rezervacija rezervacija);
}