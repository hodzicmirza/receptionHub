namespace receptionHubBackend.Controllers.Dtos;

public class RezervacijaGostDto
{
    public int Id { get; set; }
    public int RezervacijaId { get; set; }
    public int GostId { get; set; }
    public string? ImeGosta { get; set; }
    public string? PrezimeGosta { get; set; }
    public string? NazivFirme { get; set; }
    public bool JeGlavniGost { get; set; }
    public string? PosebneNapomene { get; set; }
    public string? BrojRezervacije { get; set; }
}