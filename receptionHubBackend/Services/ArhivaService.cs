using Microsoft.EntityFrameworkCore;
using receptionHubBackend.Data;
using receptionHubBackend.Models;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Services;

public class ArhivaService : IArhivaService
{
    private readonly ReceptionHubDbContext _baza;
    private readonly ILogService _logService;

    public ArhivaService(ReceptionHubDbContext baza, ILogService logService)
    {
        _baza = baza;
        _logService = logService;
    }

    // AUTOMATSKO ARHIVIRANJE - pokreće se pozivom iz kontrolera ili background servisa
    public async Task<int> ArhivirajZavrseneRezervacijeAsync()
    {
        var danas = DateTime.UtcNow.Date;
        var arhivirano = 0;

        // Pronađi sve rezervacije kojima je datum odlaska prošao
        // i koje nisu već arhivirane (status nije arhivirana)
        var zavrseneRezervacije = await _baza.Rezervacije
            .Where(r => r.DatumOdlaska < danas
                && r.Status != StatusRezervacije.Otkazana)
            .Include(r => r.GostiURezervaciji)
                .ThenInclude(rg => rg.Gost)
            .Include(r => r.Soba)
            .ToListAsync();

        foreach (var rez in zavrseneRezervacije)
        {
            await ArhivirajRezervacijuAsync(rez);
            arhivirano++;
        }

        if (arhivirano > 0)
        {
            await _logService.LogInfoAsync($"Automatski arhivirano {arhivirano} završenih rezervacija", "ArhivaService");
        }

        return arhivirano;
    }

    // RUČNO ARHIVIRANJE - poziva recepcioner
    public async Task<bool> ArhivirajRezervacijuAsync(int rezervacijaId, int arhiviraoRecepcionerId)
    {
        var rezervacija = await _baza.Rezervacije
            .Include(r => r.GostiURezervaciji)
                .ThenInclude(rg => rg.Gost)
            .Include(r => r.Soba)
            .FirstOrDefaultAsync(r => r.IdRezervacije == rezervacijaId);

        if (rezervacija == null)
        {
            await _logService.LogUpozorenjeAsync($"Pokušaj arhiviranja nepostojeće rezervacije ID: {rezervacijaId}", "ArhivaService");
            return false;
        }

        await ArhivirajRezervacijuAsync(rezervacija, arhiviraoRecepcionerId);
        return true;
    }

    // RUČNO ARHIVIRANJE GOSTA
    public async Task<bool> ArhivirajGostaAsync(int gostId, int arhiviraoRecepcionerId)
    {
        var gost = await _baza.Gosti
            .Include(g => g.RezervacijeGosta)
            .FirstOrDefaultAsync(g => g.IdGosta == gostId);

        if (gost == null)
        {
            await _logService.LogUpozorenjeAsync($"Pokušaj arhiviranja nepostojećeg gosta ID: {gostId}", "ArhivaService");
            return false;
        }

        // Provjeri da li gost ima aktivne rezervacije
        var aktivneRezervacije = gost.RezervacijeGosta?
            .Any(rg => rg.Rezervacija != null &&
                rg.Rezervacija.DatumOdlaska >= DateTime.UtcNow.Date) ?? false;

        if (aktivneRezervacije)
        {
            await _logService.LogUpozorenjeAsync(
                $"Gost ID: {gostId} ima aktivne rezervacije, ne može se arhivirati",
                "ArhivaService");
            return false;
        }

        // Arhiviraj gosta
        var arhiviraniGost = new ArhiviraniGost
        {
            OriginalniGostId = gost.IdGosta,
            Ime = gost.Ime,
            Prezime = gost.Prezime,
            TipGosta = gost.TipGosta,
            NazivFirme = gost.NazivFirme,
            KontaktOsoba = gost.KontaktOsoba,
            BrojTelefona = gost.BrojTelefona,
            Email = gost.Email,
            Drzava = gost.Drzava,
            TipDokumenta = gost.TipDokumenta,
            SlikaDokumenta = gost.SlikaDokumenta,
            VIPGost = gost.VIPGost,
            Dodatno = gost.Dodatno,
            OriginalniRecepcionerId = gost.RecepcionerId,
            OriginalnoVrijemeKreiranja = gost.VrijemeKreiranja,
            OriginalnoVrijemeAzuriranja = gost.VrijemeAzuriranja,
            DatumArhiviranja = DateTime.UtcNow,
            ArhiviraoRecepcionerId = arhiviraoRecepcionerId,
            RazlogArhiviranja = RazlogArhiviranjaGosta.ZavrsioBoravak
        };

        _baza.ArhiviraniGosti.Add(arhiviraniGost);
        _baza.Gosti.Remove(gost);
        await _baza.SaveChangesAsync();

        await _logService.LogAkcijuAsync(
            $"Gost arhiviran: {(gost.TipGosta == TipGosta.FizickoLice ? $"{gost.Ime} {gost.Prezime}" : gost.NazivFirme)}",
            arhiviraoRecepcionerId,
            gostId
        );

        return true;
    }

    // PRIVATNA METODA ZA ARHIVIRANJE REZERVACIJE
    private async Task ArhivirajRezervacijuAsync(Rezervacija rez, int? arhiviraoRecepcionerId = null)
    {
        // Arhiviraj rezervaciju
        var arhiviranaRez = new ArhiviranaRezervacija
        {
            OriginalnaRezervacijaId = rez.IdRezervacije,
            OriginalniGostId = rez.GostiURezervaciji?.FirstOrDefault()?.GostId ?? 0,
            BrojRezervacije = rez.BrojRezervacije ?? "",
            SobaId = rez.SobaId,
            BrojSobe = rez.Soba?.BrojSobe ?? "Nepoznato",
            DatumDolaska = rez.DatumDolaska,
            DatumOdlaska = rez.DatumOdlaska,
            BrojNocenja = rez.BrojNocenja,
            BrojOdraslih = rez.BrojOdraslih,
            BrojDjece = rez.BrojDjece,
            UkupnoGostiju = rez.UkupnoGostiju,
            CijenaPoNoci = rez.CijenaPoNoci,
            Popust = rez.Popust,
            UkupnaCijena = rez.UkupnaCijena,
            NacinRezervacije = rez.NacinRezervacije,
            EksterniBrojRezervacije = rez.EksterniBrojRezervacije,
            Status = rez.Status,
            Zahtjevi = rez.Zahtjevi,
            Napomena = rez.Napomena,
            OriginalniRecepcionerId = rez.RecepcionerId,
            OriginalnoVrijemeKreiranja = rez.VrijemeKreiranja,
            OriginalnoVrijemeOtkazivanja = rez.VrijemeOtkazivanja,
            OriginalniRazlogOtkazivanja = rez.RazlogOtkazivanja,
            DatumArhiviranja = DateTime.UtcNow,
            ArhiviraoRecepcionerId = arhiviraoRecepcionerId ?? rez.RecepcionerId,
            RazlogArhiviranja = RazlogArhiviranjaRezervacije.UspjesnoZavrsena
        };

        _baza.ArhiviraneRezervacije.Add(arhiviranaRez);

        // Arhiviraj sve goste iz rezervacije (samo ako nemaju drugih aktivnih rezervacija)
        if (rez.GostiURezervaciji != null)
        {
            foreach (var rg in rez.GostiURezervaciji.Where(x => x.Gost != null))
            {
                if (rg.Gost != null)
                {
                    // Provjeri da li gost ima drugih aktivnih rezervacija
                    var imaDrugeRezervacije = await _baza.Rezervacije
                        .AnyAsync(r => r.IdRezervacije != rez.IdRezervacije
                            && r.GostiURezervaciji.Any(g => g.GostId == rg.GostId)
                            && r.DatumOdlaska >= DateTime.UtcNow.Date);

                    if (!imaDrugeRezervacije)
                    {
                        // Arhiviraj gosta jer nema više aktivnih rezervacija
                        await ArhivirajGostaAsync(rg.Gost.IdGosta, arhiviraoRecepcionerId ?? rez.RecepcionerId);
                    }
                }
            }
        }

        // Obriši rezervaciju
        _baza.Rezervacije.Remove(rez);
        await _baza.SaveChangesAsync();

        await _logService.LogAkcijuAsync(
            $"Rezervacija {rez.BrojRezervacije} arhivirana",
            arhiviraoRecepcionerId ?? rez.RecepcionerId,
            null
        );
    }
}