using System.ComponentModel.DataAnnotations;

namespace receptionHubBackend.Controllers.Dtos;

public class KreirajRezervacijaGostDto
{
    [Required(ErrorMessage = "Rezervacija ID je obavezan")]
    public int RezervacijaId { get; set; }

    [Required(ErrorMessage = "Gost ID je obavezan")]
    public int GostId { get; set; }

    public bool JeGlavniGost { get; set; } = false;

    [StringLength(500, ErrorMessage = "Napomena može imati najviše 500 karaktera")]
    public string? PosebneNapomene { get; set; }
}