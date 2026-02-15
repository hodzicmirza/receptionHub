using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Models;

public class ArhiviraniGost
{
    [Key]
    public int IdArhive { get; set; }

    // Originalni ID iz tabele Gosti (da možeš pratiti)
    public int OriginalniGostId { get; set; }

    // Svi podaci o gostu (kopija iz Gost tabele)
    public string? Ime { get; set; }
    public string? Prezime { get; set; }
    public TipGosta TipGosta { get; set; }
    public string? NazivFirme { get; set; }
    public string? KontaktOsoba { get; set; }
    public string? BrojTelefona { get; set; }
    public string? Email { get; set; }
    public string? Drzava { get; set; }
    public string TipDokumenta { get; set; } = null!;
    public string SlikaDokumenta { get; set; } = null!;
    public bool VIPGost { get; set; }
    public string? Dodatno { get; set; }

    // Ko je unio originalnog gosta
    public int OriginalniRecepcionerId { get; set; }

    // Datumi
    public DateTime OriginalnoVrijemeKreiranja { get; set; }
    public DateTime? OriginalnoVrijemeAzuriranja { get; set; }

    // Podaci o arhiviranju
    public DateTime DatumArhiviranja { get; set; } = DateTime.UtcNow;
    public int ArhiviraoRecepcionerId { get; set; } 
    public RazlogArhiviranjaGosta RazlogArhiviranja { get; set; } = RazlogArhiviranjaGosta.ZavrsioBoravak;

    // Veza sa arhiviranom rezervacijom (opciono)
    public int? ArhiviranaRezervacijaId { get; set; }

    // Napomena
    [StringLength(500)]
    public string? Napomena { get; set; }
}