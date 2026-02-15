using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Models;

public class Rezervacija
{
    [Key]
    public int IdRezervacije { get; set; }

    // Broj rezervacije - jedinstveni identifikator za goste
    [Required]
    [StringLength(20)]
    public string? BrojRezervacije { get; set; } // Npr. "R-2024-0001"

    // Veze prema drugim tabelama (FOREIGN KEYS)
    [Required]
    public ICollection<RezervacijaGost> GostiURezervaciji { get; set; } = new List<RezervacijaGost>();

    [Required]
    public int SobaId { get; set; } // Gdje dolazi

    [Required]
    public int RecepcionerId { get; set; } // Ko je napravio rezervaciju

    // Datumi
    [Required(ErrorMessage = "Datum dolaska je obavezan")]
    [DataType(DataType.Date)]
    public DateTime DatumDolaska { get; set; } // Check-in

    [Required(ErrorMessage = "Datum odlaska je obavezan")]
    [DataType(DataType.Date)]
    public DateTime DatumOdlaska { get; set; } // Check-out

    // Broj noćenja (izračunato)
    public int BrojNocenja => (DatumOdlaska - DatumDolaska).Days;

    // Gosti
    [Required]
    [Range(1, 10)]
    public int BrojOdraslih { get; set; } = 1;

    [Range(0, 5)]
    public int BrojDjece { get; set; } = 0;

    public int UkupnoGostiju => BrojOdraslih + BrojDjece;

    // Cijene
    [Required]
    [DataType(DataType.Currency)]
    public decimal CijenaPoNoci { get; set; } // Cijena u trenutku rezervacije

    [DataType(DataType.Currency)]
    public decimal? Popust { get; set; } // Iznos popusta (ako ga je odobrio recepcioner)

    [DataType(DataType.Currency)]
    public decimal UkupnaCijena { get; set; } // (CijenaPoNoci * BrojNocenja) - Popust

    // Način rezervacije
    [Required]
    public NacinRezervacije NacinRezervacije { get; set; }

    // AKO JE BOOKING.COM
    public string? EksterniBrojRezervacije { get; set; }

    // Statusi
    [Required]
    public StatusRezervacije Status { get; set; } = StatusRezervacije.Potvrdjena;

    // Posebni zahtjevi
    [StringLength(500)]
    public string? Zahtjevi { get; set; }

    [StringLength(500)]
    public string? Napomena { get; set; }

    // Historija
    public DateTime VrijemeKreiranja { get; set; } = DateTime.UtcNow;
    public DateTime? VrijemeOtkazivanja { get; set; }
    public string? RazlogOtkazivanja { get; set; }
    public DateTime? VrijemeAzuriranja { get; set; }

    // Navigacijski properties (opciono - za Entity Framework)
    public virtual Soba? Soba { get; set; }
    public virtual Recepcioner? Recepcioner { get; set; }
}