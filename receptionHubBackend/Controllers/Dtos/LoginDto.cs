using System.ComponentModel.DataAnnotations;

namespace receptionHubBackend.Controllers.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Korisnicko ime je obavezno")]
    public string KorisnickoIme { get; set; }= null!;

    [Required(ErrorMessage = "Lozinka je obavezna")]
    public string Lozinka { get; set; } = null!;

}
