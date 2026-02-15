using System.ComponentModel.DataAnnotations;

namespace receptionHubBackend.Models;

public class RezervacijaGost
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RezervacijaId { get; set; }

    [Required]
    public int GostId { get; set; }

    public bool JeGlavniGost { get; set; } = false; 

    [StringLength(500)]
    public string? PosebneNapomene { get; set; } 

    // Navigacijski properties
    public virtual Rezervacija? Rezervacija { get; set; }
    public virtual Gost? Gost { get; set; }
}