using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class RezervacijaDto
{
    public int IdRezervacije { get; set; }
    public string? BrojRezervacije { get; set; }
    public int SobaId { get; set; }
    public string? BrojSobe { get; set; }
    public int RecepcionerId { get; set; }
    public string? ImeRecepcionera { get; set; }
    public DateTime DatumDolaska { get; set; }
    public DateTime DatumOdlaska { get; set; }
    public int BrojNocenja { get; set; }
    public int BrojOdraslih { get; set; }
    public int BrojDjece { get; set; }
    public int UkupnoGostiju { get; set; }
    public decimal CijenaPoNoci { get; set; }
    public decimal? Popust { get; set; }
    public decimal UkupnaCijena { get; set; }
    public NacinRezervacije NacinRezervacije { get; set; }
    public string? EksterniBrojRezervacije { get; set; }
    public StatusRezervacije Status { get; set; }
    public string? Zahtjevi { get; set; }
    public string? Napomena { get; set; }
    public DateTime VrijemeKreiranja { get; set; }
    public List<RezervacijaGostDto>? Gosti { get; set; }
    public int BrojGostiju => Gosti?.Count ?? 0;
}