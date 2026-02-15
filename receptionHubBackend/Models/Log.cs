using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Models;

public class Log
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Vrijeme { get; set; } = DateTime.UtcNow;

    [Required]
    public TipLoga Tip { get; set; }

    [Required]
    [StringLength(1000)]
    public string Poruka { get; set; } = null!;

    [StringLength(4000)]
    public string? Detalji { get; set; }

    [StringLength(200)]
    public string? Izvor { get; set; } // Kontroler, servis, middleware

    public int? RecepcionerId { get; set; }
    public int? GostId { get; set; }

    [StringLength(100)]
    public string? KorisnickoIme { get; set; } // Za slučaj da nema ID-a

    [StringLength(10)]
    public string? HttpMetoda { get; set; } // GET, POST, PUT, DELETE

    [StringLength(500)]
    public string? Putanja { get; set; }

    [Range(100, 599)]
    public int? StatusKod { get; set; }

    [StringLength(45)]
    public string? IPAdresa { get; set; }

    [StringLength(500)]
    public string? UserAgent { get; set; }

    public string? Zaglavlja { get; set; } // JSON format
    public string? TijeloZahtjeva { get; set; } // JSON format 

    public long? TrajanjeMs { get; set; } // Koliko je trajao zahtjev

    public string? TipIzuzetka { get; set; }
    public string? StackTrace { get; set; }


    [StringLength(100)]
    public string? TraceId { get; set; } // Za povezivanje više logova

    [StringLength(100)]
    public string? SessionId { get; set; }

    public string? DodatniPodaci { get; set; } 
    
}