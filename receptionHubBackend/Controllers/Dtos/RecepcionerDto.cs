using System;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class RecepcionerDto
{
    public int IdRecepcionera { get; set; }
    public string Ime { get; set; } = null!;
    public string Prezime { get; set; } = null!;
    public string KorisnickoIme { get; set; } = null!;
    public string? Email { get; set; }
    public string? BrojTelefona { get; set; }
    public TipPozicije Pozicija { get; set; }
    public bool Aktivan { get; set; }
    public DateTime DatumKreiranjaRacuna { get; set; }
    public DateTime? PosljednjiLogin { get; set; }
    public string? SlikaProfila { get; set; }
    public string? Napomena { get; set; }
}
