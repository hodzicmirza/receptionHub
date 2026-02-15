using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Models;

public class Soba
{
    [Key]
    public int IdSobe { get; set; }

    [Required(ErrorMessage = "Broj sobe je obavezan")]
    [StringLength(10, MinimumLength = 1)]
    public string BrojSobe { get; set; } = null!;

    [Required]
    public TipSobe TipSobe { get; set; }

    // Kapacitet
    [Required]
    [Range(1, 6)]
    public int MaksimalnoGostiju { get; set; }

    [Range(1, 4)]
    public int BrojKreveta { get; set; }

    // Kreveti - detaljnije
    public int? BrojBracnihKreveta { get; set; } 
    public int? BrojOdvojenihKreveta { get; set; } 
    public bool ImaDodatniKrevet { get; set; } 

    // Cijena
    [Required]
    [Range(10, 1000)]
    [DataType(DataType.Currency)]
    public decimal CijenaPoNociBAM { get; set; }

    // Opis
    [StringLength(500)]
    public string? Opis { get; set; }

    [StringLength(200)]
    public string? KratkiOpis { get; set; } 

    // Oprema - prošireno
    public bool ImaTv { get; set; } = true;
    public bool ImaKlimu { get; set; } = true;
    public bool ImaMiniBar { get; set; } = false;
    public bool ImaPogledNaGrad { get; set; } = false;
    public bool ImaWiFi { get; set; } = true;
    public bool ImaRadniSto { get; set; } = false; 
    public bool ImaFen { get; set; } = true;
    public bool ImaPeglu { get; set; } = false;
    public bool ImaKupatilo { get; set; } = true;
    public bool ImaTus { get; set; } = true;

    // Status
    [Required]
    public StatusSobe Status { get; set; } = StatusSobe.Slobodna;

    public DateTime? PlaniranoOslobadjanje { get; set; } // Za sobe koje se čiste

    // Slike
    public string? GlavnaSlika { get; set; }  

    // Napomene
    [StringLength(500)]
    public string? Napomena { get; set; }

    // Metadata
    public DateTime VrijemeKreiranja { get; set; } = DateTime.UtcNow;
    public int? KreiraoRecepcionerId { get; set; }
    public DateTime? VrijemeAzuriranja { get; set; }
    public int? AzuriraoRecepcionerId { get; set; }

}