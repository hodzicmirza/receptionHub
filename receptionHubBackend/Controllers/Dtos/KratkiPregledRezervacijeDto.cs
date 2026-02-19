using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class KratkiPregledRezervacijeDto
{
    public int IdRezervacije { get; set; }
    public string? BrojRezervacije { get; set; }
    public string? BrojSobe { get; set; }
    public DateTime DatumDolaska { get; set; }
    public DateTime DatumOdlaska { get; set; }
    public int BrojNocenja { get; set; }
    public int UkupnoGostiju { get; set; }
    public decimal UkupnaCijena { get; set; }
    public StatusRezervacije Status { get; set; }
    public string? ImeGlavnogGosta { get; set; }
}