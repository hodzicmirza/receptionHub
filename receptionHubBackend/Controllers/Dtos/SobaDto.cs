using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class SobaDto
{
    public int IdSobe { get; set; }
    public string BrojSobe { get; set; } = null!;
    public TipSobe TipSobe { get; set; }
    public int MaksimalnoGostiju { get; set; }
    public int BrojKreveta { get; set; }
    public int? BrojBracnihKreveta { get; set; }
    public int? BrojOdvojenihKreveta { get; set; }
    public bool ImaDodatniKrevet { get; set; }
    public decimal CijenaPoNociBAM { get; set; }
    public string? Opis { get; set; }
    public string? KratkiOpis { get; set; }

    // Oprema
    public bool ImaTv { get; set; }
    public bool ImaKlimu { get; set; }
    public bool ImaMiniBar { get; set; }
    public bool ImaPogledNaGrad { get; set; }
    public bool ImaWiFi { get; set; }
    public bool ImaRadniSto { get; set; }
    public bool ImaFen { get; set; }
    public bool ImaPeglu { get; set; }
    public bool ImaKupatilo { get; set; }
    public bool ImaTus { get; set; }

    // Status
    public StatusSobe Status { get; set; }
    public DateTime? PlaniranoOslobadjanje { get; set; }

    // Slike i napomene
    public string? GlavnaSlika { get; set; }
    public string? Napomena { get; set; }

    // Metadata
    public DateTime VrijemeKreiranja { get; set; }
    public int? KreiraoRecepcionerId { get; set; }
    public DateTime? VrijemeAzuriranja { get; set; }

    // Dodatno - broj aktivnih rezervacija (opciono)
    public int BrojAktivnihRezervacija { get; set; }
}