using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; } = null!;
        public DateTime Istice { get; set; }
        public string Tip { get; set; } = "Bearer";
        public string Ime { get; set; } = null!;
        public string Prezime { get; set; } = null!;
        public string KorisnickoIme { get; set; } = null!;
        public TipPozicije Pozicija { get; set; }
    }
}