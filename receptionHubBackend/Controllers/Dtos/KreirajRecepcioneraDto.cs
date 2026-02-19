using System;
using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class KreirajRecepcioneraDto
{
    [Required(ErrorMessage = "Ime je obavezno")]
    public string Ime { get; set; } = null!;

    [Required(ErrorMessage = "Prezime je obavezno")]
    public string Prezime { get; set; } = null!;

    [Required(ErrorMessage = "Korisničko ime je obavezno")]
    public string KorisnickoIme { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Email nije ispravnog formata")]
    public string? Email { get; set; }

    public string? BrojTelefona { get; set; }

    [Required(ErrorMessage = "Pozicija je obavezna")]
    public TipPozicije Pozicija { get; set; }

    [Required(ErrorMessage = "Lozinka je obavezna")]
    [MinLength(6, ErrorMessage = "Lozinka mora imati najmanje 6 karaktera")]
    public string Lozinka { get; set; } = null!;

    public string? SlikaProfila { get; set; }
    public string? Napomena { get; set; }
}
