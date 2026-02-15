using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Models;

public class ArhiviranaRezervacija
{
    [Key]
    public int IdArhive { get; set; }

    // Originalni ID iz tabele Rezervacije
    public int OriginalnaRezervacijaId { get; set; }

    // Originalni ID gosta (iz Gost tabele)
    public int OriginalniGostId { get; set; }

    // Podaci o rezervaciji (kopija)
    public string BrojRezervacije { get; set; } = null!;
    public int SobaId { get; set; }
    public string BrojSobe { get; set; } = null!; // Dodajemo broj sobe direktno
    public DateTime DatumDolaska { get; set; }
    public DateTime DatumOdlaska { get; set; }
    public int BrojNocenja { get; set; }
    public int BrojOdraslih { get; set; }
    public int BrojDjece { get; set; }
    public int UkupnoGostiju { get; set; }
    public decimal CijenaPoNoci { get; set; }
    public decimal? Popust { get; set; }
    public decimal UkupnaCijena { get; set; }
    public NacinRezervacije NacinRezervacije { get; set; }
    public string? EksterniBrojRezervacije { get; set; }
    public StatusRezervacije Status { get; set; }
    public string? Zahtjevi { get; set; }
    public string? Napomena { get; set; }

    // Ko je napravio originalnu rezervaciju
    public int OriginalniRecepcionerId { get; set; }
    public DateTime OriginalnoVrijemeKreiranja { get; set; }

    // Datumi otkazivanja (ako je otkazano)
    public DateTime? OriginalnoVrijemeOtkazivanja { get; set; }
    public string? OriginalniRazlogOtkazivanja { get; set; }

    // Podaci o arhiviranju
    public DateTime DatumArhiviranja { get; set; } = DateTime.UtcNow;
    public int ArhiviraoRecepcionerId { get; set; }
    public RazlogArhiviranjaRezervacije RazlogArhiviranja { get; set; } = RazlogArhiviranjaRezervacije.UspjesnoZavrsena;

}