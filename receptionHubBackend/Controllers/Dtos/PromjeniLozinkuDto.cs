using System;
using System.ComponentModel.DataAnnotations;

namespace receptionHubBackend.Controllers.Dtos;

public class PromjeniLozinkuDto
{
    [Required(ErrorMessage = "Stara lozinka je obavezna")]
    public string StaraLozinka { get; set; } = null!;

    [Required(ErrorMessage = "Nova lozinka je obavezna")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Nova lozinka mora imati najmanje 6 karaktera")]
    public string NovaLozinka { get; set; } = null!;

    [Required(ErrorMessage = "Potvrda lozinke je obavezna")]
    [Compare("NovaLozinka", ErrorMessage = "Lozinke se ne poklapaju")]
    public string PotvrdaLozinke { get; set; } = null!;
}
