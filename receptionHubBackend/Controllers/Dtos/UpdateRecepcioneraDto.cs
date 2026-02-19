using System;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Controllers.Dtos;

public class UpdateRecepcioneraDto
{
    public string? Ime { get; set; }
    public string? Prezime { get; set; }
    public string? Email { get; set; }
    public string? BrojTelefona { get; set; }
    public TipPozicije? Pozicija { get; set; } // Samo admin
    public bool? Aktivan { get; set; } // Samo admin
    public string? SlikaProfila { get; set; }
    public string? Napomena { get; set; }
}
