using iTextSharp.text;
using iTextSharp.text.pdf;
using receptionHubBackend.Models;
using receptionHubBackend.Models.Enums;

namespace receptionHubBackend.Services;

public class PdfService : IPdfService
{
    public async Task<byte[]> GenerisiGostPdfAsync(Gost gost)
    {
        using (var stream = new MemoryStream())
        {
            // Kreiraj dokument
            var document = new Document(PageSize.A4, 50, 50, 25, 25);
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            // Dodaj font
            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            var fontNaslov = new Font(baseFont, 18f, Font.BOLD, BaseColor.DarkGray);
            var fontPodnaslov = new Font(baseFont, 14f, Font.BOLD, BaseColor.Gray);
            var fontTekst = new Font(baseFont, 12f, Font.NORMAL, BaseColor.Black);

            // Naslov
            var naslov = new Paragraph("Podaci o gostu", fontNaslov);
            naslov.Alignment = Element.ALIGN_CENTER;
            naslov.SpacingAfter = 20f;
            document.Add(naslov);

            // Kreiraj tabelu
            var tabela = new PdfPTable(2);
            tabela.WidthPercentage = 100f;
            tabela.SetWidths(new float[] { 40f, 60f });

            // Funkcija za dodavanje reda
            void DodajRed(string labela, string? vrijednost)
            {
                if (!string.IsNullOrEmpty(vrijednost))
                {
                    var cell1 = new PdfPCell(new Phrase(labela, fontTekst)) { Border = Rectangle.NO_BORDER };
                    var cell2 = new PdfPCell(new Phrase(vrijednost, fontTekst)) { Border = Rectangle.NO_BORDER };
                    tabela.AddCell(cell1);
                    tabela.AddCell(cell2);
                }
            }

            // Dodaj podatke
            DodajRed("ID Gosta:", gost.IdGosta.ToString());

            if (gost.TipGosta == TipGosta.FizickoLice)
            {
                DodajRed("Ime i prezime:", $"{gost.Ime} {gost.Prezime}");
            }
            else
            {
                DodajRed("Naziv firme:", gost.NazivFirme);
                DodajRed("Kontakt osoba:", gost.KontaktOsoba);
            }

            DodajRed("Tip gosta:", gost.TipGosta.ToString());
            DodajRed("Broj telefona:", gost.BrojTelefona);
            DodajRed("Email:", gost.Email);
            DodajRed("Država:", gost.Drzava);
            DodajRed("Tip dokumenta:", gost.TipDokumenta);
            DodajRed("VIP gost:", gost.VIPGost ? "DA" : "NE");
            DodajRed("Dodatno:", gost.Dodatno);
            DodajRed("Datum kreiranja:", gost.VrijemeKreiranja.ToString("dd.MM.yyyy HH:mm"));
            DodajRed("Kreirao recepcioner ID:", gost.RecepcionerId.ToString());

            document.Add(tabela);
            document.Close();

            return stream.ToArray();
        }
    }

    public async Task<byte[]> GenerisiListuGostijuPdfAsync(List<Gost> gosti)
    {
        using (var stream = new MemoryStream())
        {
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            var fontNaslov = new Font(baseFont, 16f, Font.BOLD);
            var fontZaglavlje = new Font(baseFont, 10f, Font.BOLD);
            var fontSadrzaj = new Font(baseFont, 9f, Font.NORMAL);

            // Naslov
            var naslov = new Paragraph($"Lista gostiju - {DateTime.Now:dd.MM.yyyy}", fontNaslov);
            naslov.Alignment = Element.ALIGN_CENTER;
            naslov.SpacingAfter = 20f;
            document.Add(naslov);

            // Tabela
            var tabela = new PdfPTable(8);
            tabela.WidthPercentage = 100f;

            // Zaglavlje
            string[] zaglavlja = { "ID", "Ime/Prezime", "Telefon", "Email", "Država", "Tip", "VIP", "Datum" };
            foreach (var zag in zaglavlja)
            {
                var cell = new PdfPCell(new Phrase(zag, fontZaglavlje))
                {
                    BackgroundColor = BaseColor.LightGray,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                tabela.AddCell(cell);
            }

            // Podaci
            foreach (var gost in gosti)
            {
                tabela.AddCell(new Phrase(gost.IdGosta.ToString(), fontSadrzaj));
                tabela.AddCell(new Phrase(gost.TipGosta == TipGosta.FizickoLice 
                    ? $"{gost.Ime} {gost.Prezime}" 
                    : gost.NazivFirme ?? "-", fontSadrzaj));
                tabela.AddCell(new Phrase(gost.BrojTelefona ?? "-", fontSadrzaj));
                tabela.AddCell(new Phrase(gost.Email ?? "-", fontSadrzaj));
                tabela.AddCell(new Phrase(gost.Drzava ?? "-", fontSadrzaj));
                tabela.AddCell(new Phrase(gost.TipGosta.ToString(), fontSadrzaj));
                tabela.AddCell(new Phrase(gost.VIPGost ? "✓" : "✗", fontSadrzaj));
                tabela.AddCell(new Phrase(gost.VrijemeKreiranja.ToString("dd.MM.yyyy"), fontSadrzaj));
            }

            document.Add(tabela);
            document.Close();

            return stream.ToArray();
        }
    }

    public async Task<byte[]> GenerisiRezervacijuPdfAsync(Rezervacija rezervacija)
    {
        using (var stream = new MemoryStream())
        {
            var document = new Document(PageSize.A4, 50, 50, 25, 25);
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            var fontNaslov = new Font(baseFont, 20f, Font.BOLD, BaseColor.DarkGray);
            var fontPodnaslov = new Font(baseFont, 14f, Font.BOLD);
            var fontTekst = new Font(baseFont, 12f, Font.NORMAL);

            // Zaglavlje
            var naslov = new Paragraph("POTVRDA REZERVACIJE", fontNaslov);
            naslov.Alignment = Element.ALIGN_CENTER;
            naslov.SpacingAfter = 20f;
            document.Add(naslov);

            // Broj rezervacije
            var broj = new Paragraph($"Broj: {rezervacija.BrojRezervacije}", fontPodnaslov);
            broj.SpacingAfter = 20f;
            document.Add(broj);

            // Tabela
            var tabela = new PdfPTable(2);
            tabela.WidthPercentage = 100f;
            tabela.SetWidths(new float[] { 30f, 70f });

            void DodajRed(string labela, string vrijednost)
            {
                var cell1 = new PdfPCell(new Phrase(labela, fontTekst)) { Border = Rectangle.NO_BORDER };
                var cell2 = new PdfPCell(new Phrase(vrijednost, fontTekst)) { Border = Rectangle.NO_BORDER };
                tabela.AddCell(cell1);
                tabela.AddCell(cell2);
            }

            // Podaci o rezervaciji
            DodajRed("Soba:", rezervacija.Soba?.BrojSobe ?? "-");
            DodajRed("Datum dolaska:", rezervacija.DatumDolaska.ToString("dd.MM.yyyy"));
            DodajRed("Datum odlaska:", rezervacija.DatumOdlaska.ToString("dd.MM.yyyy"));
            DodajRed("Broj noćenja:", rezervacija.BrojNocenja.ToString());
            DodajRed("Odrasli:", rezervacija.BrojOdraslih.ToString());
            DodajRed("Djeca:", rezervacija.BrojDjece.ToString());
            DodajRed("Cijena po noći:", $"{rezervacija.CijenaPoNoci:F2} BAM");
            if (rezervacija.Popust.HasValue)
                DodajRed("Popust:", $"{rezervacija.Popust:F2} BAM");
            DodajRed("Ukupno:", $"{rezervacija.UkupnaCijena:F2} BAM");
            DodajRed("Način rezervacije:", rezervacija.NacinRezervacije.ToString());
            DodajRed("Status:", rezervacija.Status.ToString());
            if (!string.IsNullOrEmpty(rezervacija.Zahtjevi))
                DodajRed("Posebni zahtjevi:", rezervacija.Zahtjevi);

            document.Add(tabela);

            // Gosti
            if (rezervacija.GostiURezervaciji != null && rezervacija.GostiURezervaciji.Any())
            {
                document.Add(new Paragraph("\nGosti:", fontPodnaslov));

                var tabelaGosti = new PdfPTable(4);
                tabelaGosti.WidthPercentage = 100f;

                string[] zaglavlja = { "Ime i prezime", "Tip gosta", "Glavni gost", "Napomena" };
                foreach (var zag in zaglavlja)
                {
                    var cell = new PdfPCell(new Phrase(zag, fontTekst))
                    {
                        BackgroundColor = BaseColor.LightGray,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    tabelaGosti.AddCell(cell);
                }

                foreach (var rg in rezervacija.GostiURezervaciji.Where(x => x.Gost != null))
                {
                    var ime = rg.Gost?.TipGosta == TipGosta.FizickoLice
                        ? $"{rg.Gost?.Ime} {rg.Gost?.Prezime}"
                        : rg.Gost?.NazivFirme ?? "-";

                    tabelaGosti.AddCell(new Phrase(ime, fontTekst));
                    tabelaGosti.AddCell(new Phrase(rg.JeGlavniGost ? "DA" : "NE", fontTekst));
                    tabelaGosti.AddCell(new Phrase(rg.PosebneNapomene ?? "-", fontTekst));
                }

                document.Add(tabelaGosti);
            }

            document.Close();
            return stream.ToArray();
        }
    }
}