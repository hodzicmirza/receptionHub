using System.ComponentModel.DataAnnotations;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class StatusSobeDto
{
    [Required(ErrorMessage = "Status je obavezan")]
    public StatusSobe Status { get; set; }

    public DateTime? PlaniranoOslobadjanje { get; set; }
}