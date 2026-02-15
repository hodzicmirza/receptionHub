namespace receptionHubBackend.Models.Enums;

public enum RazlogArhiviranjaRezervacije
{
    UspjesnoZavrsena = 1,        // Gost odsjeo i otišao - uredno
    OtkazanaOdGosta = 2,         // Gost otkazao prije dolaska
    OtkazanaOdHotela = 3,        // Hotel otkazao (npr. renoviranje, dvostruka rezervacija)
    NijeSePojavio = 4,           // No-show - gost nije došao
    PrijevremeniOdlazak = 5,     // Gost otišao ranije
    ProduzenBoravak = 6,         // Gost produžio, pa arhiviramo originalni dio
    PremjestenaUArhivu = 7,      // Sistematsko arhiviranje starih podataka
    ObrisanaOdStraneAdmina = 8   // Admin ručno arhivirao
}