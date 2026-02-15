using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Models;

public class Gost
{
    [Key]
    public int IdGosta { get; set; }

    [Required]
    public TipGosta TipGosta { get; set; } = TipGosta.FizickoLice;

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Ime mora biti između 2 i 50 karaktera")]
    public string? Ime { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Prezime mora biti između 2 i 50 karaktera")]
    public string? Prezime { get; set; }


    // ZA PRAVNA LICA (FIRME)
    public string? NazivFirme { get; set; }
    public string? KontaktOsoba { get; set; }

    // Kontakt
    public string? BrojTelefona { get; set; }

    [EmailAddress(ErrorMessage = "Email nije ispravnog formata")]
    [StringLength(100)]
    public string? Email { get; set; }

    // Adresa odakle dolazi
    public string? Drzava { get; set; }

    // Dokumenti
    [Required(ErrorMessage = "Tip dokumenta je obavezan")]
    public string TipDokumenta { get; set; } = null!;
    public string SlikaDokumenta { get; set; } = null!;

    // VIP status
    public bool VIPGost { get; set; } = false;
    public string? Dodatno { get; set; }
    // Metadata
    public DateTime VrijemeKreiranja { get; set; } = DateTime.UtcNow;

    [Required]
    public int RecepcionerId { get; set; } // Koji recepcioner je unio gosta
    public DateTime? VrijemeAzuriranja { get; set; }

    // Na kraj Gost modela, dodaj:
    public virtual ICollection<RezervacijaGost> RezervacijeGosta { get; set; } = new List<RezervacijaGost>();

}