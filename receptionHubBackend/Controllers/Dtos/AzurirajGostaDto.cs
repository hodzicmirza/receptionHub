using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class AzurirajGostaDto
{
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ime mora biti između 2 i 50 karaktera")]
    public string? Ime { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Prezime mora biti između 2 i 50 karaktera")]
    public string? Prezime { get; set; }

    [StringLength(100, ErrorMessage = "Naziv firme može imati najviše 100 karaktera")]
    public string? NazivFirme { get; set; }

    [StringLength(100, ErrorMessage = "Kontakt osoba može imati najviše 100 karaktera")]
    public string? KontaktOsoba { get; set; }

    [Phone(ErrorMessage = "Broj telefona nije ispravnog formata")]
    [StringLength(20, ErrorMessage = "Broj telefona može imati najviše 20 karaktera")]
    public string? BrojTelefona { get; set; }

    [EmailAddress(ErrorMessage = "Email nije ispravnog formata")]
    [StringLength(100, ErrorMessage = "Email može imati najviše 100 karaktera")]
    public string? Email { get; set; }

    [StringLength(100, ErrorMessage = "Država može imati najviše 100 karaktera")]
    public string? Drzava { get; set; }

    [StringLength(50, ErrorMessage = "Tip dokumenta može imati najviše 50 karaktera")]
    public string? TipDokumenta { get; set; }

    [StringLength(500, ErrorMessage = "Putanja do slike je preduga")]
    public string? SlikaDokumenta { get; set; }

    public bool? VIPGost { get; set; }

    [StringLength(500, ErrorMessage = "Napomena može imati najviše 500 karaktera")]
    public string? Dodatno { get; set; }
}