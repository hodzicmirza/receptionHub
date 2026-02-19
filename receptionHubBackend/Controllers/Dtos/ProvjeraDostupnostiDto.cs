using System.ComponentModel.DataAnnotations;

namespace receptionHubBackend.Controllers.Dtos;

public class ProvjeraDostupnostiDto
{
    [Required(ErrorMessage = "Soba ID je obavezan")]
    public int SobaId { get; set; }

    [Required(ErrorMessage = "Datum dolaska je obavezan")]
    [DataType(DataType.Date)]
    public DateTime DatumDolaska { get; set; }

    [Required(ErrorMessage = "Datum odlaska je obavezan")]
    [DataType(DataType.Date)]
    public DateTime DatumOdlaska { get; set; }
}