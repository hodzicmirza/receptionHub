namespace receptionHubBackend.Models.Enums;

public enum RazlogArhiviranjaGosta
{
    ZavrsioBoravak = 1,           // Gost je završio boravak i otišao
    OtkazanoBezDolaska = 2,      // Rezervacija otkazana, gost nije ni došao
    NijeSePojavio = 3,           // No-show - gost nije došao, a nije otkazao
    PremjestenUArhivu = 4,       // Sistematsko arhiviranje starih podataka
    ObrisanOdStraneAdmina = 5,   // Admin ručno arhivirao/obrisao
}