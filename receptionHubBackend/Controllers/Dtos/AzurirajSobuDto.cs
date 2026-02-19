using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class AzurirajSobuDto
{
    [StringLength(10, MinimumLength = 1, ErrorMessage = "Broj sobe mora biti između 1 i 10 karaktera")]
    public string? BrojSobe { get; set; }

    public TipSobe? TipSobe { get; set; }

    [Range(1, 6, ErrorMessage = "Maksimalan broj gostiju mora biti između 1 i 6")]
    public int? MaksimalnoGostiju { get; set; }

    [Range(1, 4, ErrorMessage = "Broj kreveta mora biti između 1 i 4")]
    public int? BrojKreveta { get; set; }

    public int? BrojBracnihKreveta { get; set; }
    public int? BrojOdvojenihKreveta { get; set; }
    public bool? ImaDodatniKrevet { get; set; }

    [Range(10, 1000, ErrorMessage = "Cijena mora biti između 10 i 1000 BAM")]
    [DataType(DataType.Currency)]
    public decimal? CijenaPoNociBAM { get; set; }

    [StringLength(500, ErrorMessage = "Opis može imati najviše 500 karaktera")]
    public string? Opis { get; set; }

    [StringLength(200, ErrorMessage = "Kratki opis može imati najviše 200 karaktera")]
    public string? KratkiOpis { get; set; }

    // Oprema
    public bool? ImaTv { get; set; }
    public bool? ImaKlimu { get; set; }
    public bool? ImaMiniBar { get; set; }
    public bool? ImaPogledNaGrad { get; set; }
    public bool? ImaWiFi { get; set; }
    public bool? ImaRadniSto { get; set; }
    public bool? ImaFen { get; set; }
    public bool? ImaPeglu { get; set; }
    public bool? ImaKupatilo { get; set; }
    public bool? ImaTus { get; set; }

    // Status
    public StatusSobe? Status { get; set; }
    public DateTime? PlaniranoOslobadjanje { get; set; }

    public string? GlavnaSlika { get; set; }

    [StringLength(500, ErrorMessage = "Napomena može imati najviše 500 karaktera")]
    public string? Napomena { get; set; }
}