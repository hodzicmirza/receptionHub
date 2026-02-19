using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Models.DTOs;

public class LogDto
{
    public int Id { get; set; }
    public DateTime Vrijeme { get; set; }
    public TipLoga Tip { get; set; }
    public string Poruka { get; set; } = null!;
    public string? Detalji { get; set; }
    public string? Izvor { get; set; }
    public int? RecepcionerId { get; set; }
    public int? GostId { get; set; }
    public string? KorisnickoIme { get; set; }
    public string? HttpMetoda { get; set; }
    public string? Putanja { get; set; }
    public int? StatusKod { get; set; }
    public string? IPAdresa { get; set; }
    public string? UserAgent { get; set; }
    public string? Zaglavlja { get; set; }
    public string? TijeloZahtjeva { get; set; }
    public long? TrajanjeMs { get; set; }
    public string? TipIzuzetka { get; set; }
    public string? StackTrace { get; set; }
    public string? TraceId { get; set; }
    public string? SessionId { get; set; }
    public string? DodatniPodaci { get; set; }
}