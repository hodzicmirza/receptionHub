using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class KreirajRezervacijuDto
{
    [Required(ErrorMessage = "Soba je obavezna")]
    public int SobaId { get; set; }

    [Required(ErrorMessage = "Datum dolaska je obavezan")]
    [DataType(DataType.Date)]
    public DateTime DatumDolaska { get; set; }

    [Required(ErrorMessage = "Datum odlaska je obavezan")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(KreirajRezervacijuDto), nameof(ValidirajDatume))]
    public DateTime DatumOdlaska { get; set; }

    [Required]
    [Range(1, 10)]
    public int BrojOdraslih { get; set; } = 1;

    [Range(0, 5)]
    public int BrojDjece { get; set; } = 0;

    [Required]
    [Range(0, 10000)]
    [DataType(DataType.Currency)]
    public decimal CijenaPoNoci { get; set; }

    [Range(0, 10000)]
    [DataType(DataType.Currency)]
    public decimal? Popust { get; set; }

    [Required]
    public NacinRezervacije NacinRezervacije { get; set; }

    public string? EksterniBrojRezervacije { get; set; }

    [StringLength(500)]
    public string? Zahtjevi { get; set; }

    [StringLength(500)]
    public string? Napomena { get; set; }

    // Lista ID-jeva gostiju koji će biti dodati u rezervaciju
    public List<int>? GostIds { get; set; }

    // Opciono - glavni gost
    public int? GlavniGostId { get; set; }

    public static ValidationResult? ValidirajDatume(DateTime odlaska, ValidationContext context)
    {
        var instance = (KreirajRezervacijuDto)context.ObjectInstance;

        if (odlaska <= instance.DatumDolaska)
        {
            return new ValidationResult("Datum odlaska mora biti nakon datuma dolaska");
        }

        return ValidationResult.Success;
    }
}