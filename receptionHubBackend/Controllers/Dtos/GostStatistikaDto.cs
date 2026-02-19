namespace receptionHubBackend.Controllers.Dtos;

public class GostStatistikaDto
{
    public int Ukupno { get; set; }
    public int Danas { get; set; }
    public int OvajMjesec { get; set; }
    public int FizickaLica { get; set; }
    public int PravnaLica { get; set; }
    public int VIP { get; set; }
    public List<DrzavaStatistika> TopDrzave { get; set; } = new();
}

public class DrzavaStatistika
{
    public string Drzava { get; set; } = null!;
    public int Broj { get; set; }
}