using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class AzurirajRezervacijuDto
{
    public int? SobaId { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DatumDolaska { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DatumOdlaska { get; set; }

    [Range(1, 10)]
    public int? BrojOdraslih { get; set; }

    [Range(0, 5)]
    public int? BrojDjece { get; set; }

    [Range(0, 10000)]
    [DataType(DataType.Currency)]
    public decimal? CijenaPoNoci { get; set; }

    [Range(0, 10000)]
    [DataType(DataType.Currency)]
    public decimal? Popust { get; set; }

    public NacinRezervacije? NacinRezervacije { get; set; }

    public string? EksterniBrojRezervacije { get; set; }

    public StatusRezervacije? Status { get; set; }

    [StringLength(500)]
    public string? Zahtjevi { get; set; }

    [StringLength(500)]
    public string? Napomena { get; set; }
}