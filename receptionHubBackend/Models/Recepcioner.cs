using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Models;

public class Recepcioner
{
    [Key]
    public int IdRecepcionera { get; set; }

    // Lični podaci
    [Required(ErrorMessage = "Ime je obavezno")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ime mora biti između 2 i 50 karaktera")]
    public string Ime { get; set; } = null!;

    [Required(ErrorMessage = "Prezime je obavezno")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Prezime mora biti između 2 i 50 karaktera")]
    public string Prezime { get; set; } = null!;

    // Kontakt
    public string? BrojTelefona { get; set; }

    [EmailAddress(ErrorMessage = "Email nije ispravnog formata")]
    [StringLength(100)]
    public string? Email { get; set; }

    // Korisnicki podaci
    [Required(ErrorMessage = "Korisnicko ime recepcionera je obavezno")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Korisnicko ime recepcionera mora biti između 2 i 50 karaktera")]
    public string KorisnickoIme { get; set; } = null!;

    [Required(ErrorMessage = "Lozinka je obavezna")]
    public string LozinkaHash { get; set; } = null!;

    public bool Aktivan { get; set; } = true;

    public DateTime? PosljednjiLogin { get; set; }

    [Required]
    public TipPozicije Pozicija { get; set; } = TipPozicije.Recepcioner;

    // Metadata
    public DateTime DatumKreiranjaRacuna { get; set; } = DateTime.UtcNow;

    public string? SlikaProfila { get; set; }

    public string? Napomena { get; set; }

    public ICollection<Rezervacija>? Rezervacije { get; set; }

}