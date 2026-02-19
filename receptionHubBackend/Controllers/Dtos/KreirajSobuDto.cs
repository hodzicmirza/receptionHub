using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class KreirajSobuDto
{
    [Required(ErrorMessage = "Broj sobe je obavezan")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "Broj sobe mora biti između 1 i 10 karaktera")]
    public string BrojSobe { get; set; } = null!;

    [Required(ErrorMessage = "Tip sobe je obavezan")]
    public TipSobe TipSobe { get; set; }

    [Required(ErrorMessage = "Maksimalan broj gostiju je obavezan")]
    [Range(1, 6, ErrorMessage = "Maksimalan broj gostiju mora biti između 1 i 6")]
    public int MaksimalnoGostiju { get; set; }

    [Range(1, 4, ErrorMessage = "Broj kreveta mora biti između 1 i 4")]
    public int BrojKreveta { get; set; }

    public int? BrojBracnihKreveta { get; set; }
    public int? BrojOdvojenihKreveta { get; set; }
    public bool ImaDodatniKrevet { get; set; }

    [Required(ErrorMessage = "Cijena je obavezna")]
    [Range(10, 1000, ErrorMessage = "Cijena mora biti između 10 i 1000 BAM")]
    [DataType(DataType.Currency)]
    public decimal CijenaPoNociBAM { get; set; }

    [StringLength(500, ErrorMessage = "Opis može imati najviše 500 karaktera")]
    public string? Opis { get; set; }

    [StringLength(200, ErrorMessage = "Kratki opis može imati najviše 200 karaktera")]
    public string? KratkiOpis { get; set; }

    // Oprema - sve je opciono, ali ima default vrijednosti u modelu
    public bool ImaTv { get; set; } = true;
    public bool ImaKlimu { get; set; } = true;
    public bool ImaMiniBar { get; set; } = false;
    public bool ImaPogledNaGrad { get; set; } = false;
    public bool ImaWiFi { get; set; } = true;
    public bool ImaRadniSto { get; set; } = false;
    public bool ImaFen { get; set; } = true;
    public bool ImaPeglu { get; set; } = false;
    public bool ImaKupatilo { get; set; } = true;
    public bool ImaTus { get; set; } = true;

    public string? GlavnaSlika { get; set; }

    [StringLength(500, ErrorMessage = "Napomena može imati najviše 500 karaktera")]
    public string? Napomena { get; set; }
}