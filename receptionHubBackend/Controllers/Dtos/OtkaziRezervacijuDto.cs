using System.ComponentModel.DataAnnotations;

namespace receptionHubBackend.Controllers.Dtos;

public class OtkaziRezervacijuDto
{
    [Required(ErrorMessage = "Razlog otkazivanja je obavezan")]
    [StringLength(500, ErrorMessage = "Razlog može imati najviše 500 karaktera")]
    public string Razlog { get; set; } = null!;
}