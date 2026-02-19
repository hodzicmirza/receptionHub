using System.ComponentModel.DataAnnotations;

namespace receptionHubBackend.Controllers.Dtos;

public class AzurirajRezervacijaGostDto
{
    public bool? JeGlavniGost { get; set; }

    [StringLength(500, ErrorMessage = "Napomena može imati najviše 500 karaktera")]
    public string? PosebneNapomene { get; set; }
}