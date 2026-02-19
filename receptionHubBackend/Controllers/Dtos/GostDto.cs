using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class GostDto
{
    public int IdGosta { get; set; }
    public TipGosta TipGosta { get; set; }
    public string? Ime { get; set; }
    public string? Prezime { get; set; }
    public string? NazivFirme { get; set; }
    public string? KontaktOsoba { get; set; }
    public string? BrojTelefona { get; set; }
    public string? Email { get; set; }
    public string? Drzava { get; set; }
    public string TipDokumenta { get; set; } = null!;
    public string? SlikaDokumenta { get; set; }
    public bool VIPGost { get; set; }
    public string? Dodatno { get; set; }
    public DateTime VrijemeKreiranja { get; set; }
    public int RecepcionerId { get; set; }
    public int BrojRezervacija { get; set; }
}