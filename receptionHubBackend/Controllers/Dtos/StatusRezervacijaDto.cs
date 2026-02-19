using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class StatusRezervacijeDto
{
    [Required(ErrorMessage = "Status je obavezan")]
    public StatusRezervacije Status { get; set; }

    [StringLength(500)]
    public string? Razlog { get; set; }
}