using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace receptionHubBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArhiviraneRezervacije",
                columns: table => new
                {
                    IdArhive = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginalnaRezervacijaId = table.Column<int>(type: "integer", nullable: false),
                    OriginalniGostId = table.Column<int>(type: "integer", nullable: false),
                    BrojRezervacije = table.Column<string>(type: "text", nullable: false),
                    SobaId = table.Column<int>(type: "integer", nullable: false),
                    BrojSobe = table.Column<string>(type: "text", nullable: false),
                    DatumDolaska = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatumOdlaska = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BrojNocenja = table.Column<int>(type: "integer", nullable: false),
                    BrojOdraslih = table.Column<int>(type: "integer", nullable: false),
                    BrojDjece = table.Column<int>(type: "integer", nullable: false),
                    UkupnoGostiju = table.Column<int>(type: "integer", nullable: false),
                    CijenaPoNoci = table.Column<decimal>(type: "numeric", nullable: false),
                    Popust = table.Column<decimal>(type: "numeric", nullable: true),
                    UkupnaCijena = table.Column<decimal>(type: "numeric", nullable: false),
                    NacinRezervacije = table.Column<int>(type: "integer", nullable: false),
                    EksterniBrojRezervacije = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Zahtjevi = table.Column<string>(type: "text", nullable: true),
                    Napomena = table.Column<string>(type: "text", nullable: true),
                    OriginalniRecepcionerId = table.Column<int>(type: "integer", nullable: false),
                    OriginalnoVrijemeKreiranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OriginalnoVrijemeOtkazivanja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OriginalniRazlogOtkazivanja = table.Column<string>(type: "text", nullable: true),
                    DatumArhiviranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArhiviraoRecepcionerId = table.Column<int>(type: "integer", nullable: false),
                    RazlogArhiviranja = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArhiviraneRezervacije", x => x.IdArhive);
                });

            migrationBuilder.CreateTable(
                name: "Gosti",
                columns: table => new
                {
                    IdGosta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipGosta = table.Column<int>(type: "integer", nullable: false),
                    Ime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Prezime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    NazivFirme = table.Column<string>(type: "text", nullable: true),
                    KontaktOsoba = table.Column<string>(type: "text", nullable: true),
                    BrojTelefona = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Drzava = table.Column<string>(type: "text", nullable: true),
                    TipDokumenta = table.Column<string>(type: "text", nullable: false),
                    SlikaDokumenta = table.Column<string>(type: "text", nullable: false),
                    VIPGost = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Dodatno = table.Column<string>(type: "text", nullable: true),
                    VrijemeKreiranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    RecepcionerId = table.Column<int>(type: "integer", nullable: false),
                    VrijemeAzuriranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gosti", x => x.IdGosta);
                });

            migrationBuilder.CreateTable(
                name: "Logovi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Vrijeme = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Tip = table.Column<int>(type: "integer", nullable: false),
                    Poruka = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Detalji = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Izvor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RecepcionerId = table.Column<int>(type: "integer", nullable: true),
                    GostId = table.Column<int>(type: "integer", nullable: true),
                    KorisnickoIme = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HttpMetoda = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Putanja = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StatusKod = table.Column<int>(type: "integer", nullable: true),
                    IPAdresa = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Zaglavlja = table.Column<string>(type: "text", nullable: true),
                    TijeloZahtjeva = table.Column<string>(type: "text", nullable: true),
                    TrajanjeMs = table.Column<long>(type: "bigint", nullable: true),
                    TipIzuzetka = table.Column<string>(type: "text", nullable: true),
                    StackTrace = table.Column<string>(type: "text", nullable: true),
                    TraceId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DodatniPodaci = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logovi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Recepcioneri",
                columns: table => new
                {
                    IdRecepcionera = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BrojTelefona = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    KorisnickoIme = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LozinkaHash = table.Column<string>(type: "text", nullable: false),
                    Aktivan = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    PosljednjiLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Pozicija = table.Column<int>(type: "integer", nullable: false),
                    DatumKreiranjaRacuna = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    SlikaProfila = table.Column<string>(type: "text", nullable: true),
                    Napomena = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepcioneri", x => x.IdRecepcionera);
                });

            migrationBuilder.CreateTable(
                name: "Sobe",
                columns: table => new
                {
                    IdSobe = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BrojSobe = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TipSobe = table.Column<int>(type: "integer", nullable: false),
                    MaksimalnoGostiju = table.Column<int>(type: "integer", nullable: false),
                    BrojKreveta = table.Column<int>(type: "integer", nullable: false),
                    BrojBracnihKreveta = table.Column<int>(type: "integer", nullable: true),
                    BrojOdvojenihKreveta = table.Column<int>(type: "integer", nullable: true),
                    ImaDodatniKrevet = table.Column<bool>(type: "boolean", nullable: false),
                    CijenaPoNociBAM = table.Column<decimal>(type: "numeric", nullable: false),
                    Opis = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    KratkiOpis = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ImaTv = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ImaKlimu = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ImaMiniBar = table.Column<bool>(type: "boolean", nullable: false),
                    ImaPogledNaGrad = table.Column<bool>(type: "boolean", nullable: false),
                    ImaWiFi = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ImaRadniSto = table.Column<bool>(type: "boolean", nullable: false),
                    ImaFen = table.Column<bool>(type: "boolean", nullable: false),
                    ImaPeglu = table.Column<bool>(type: "boolean", nullable: false),
                    ImaKupatilo = table.Column<bool>(type: "boolean", nullable: false),
                    ImaTus = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    PlaniranoOslobadjanje = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GlavnaSlika = table.Column<string>(type: "text", nullable: true),
                    Napomena = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    VrijemeKreiranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KreiraoRecepcionerId = table.Column<int>(type: "integer", nullable: true),
                    VrijemeAzuriranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AzuriraoRecepcionerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sobe", x => x.IdSobe);
                    table.CheckConstraint("CK_Soba_Cijena", "\"CijenaPoNociBAM\" >= 0");
                });

            migrationBuilder.CreateTable(
                name: "ArhiviraniGosti",
                columns: table => new
                {
                    IdArhive = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginalniGostId = table.Column<int>(type: "integer", nullable: false),
                    Ime = table.Column<string>(type: "text", nullable: true),
                    Prezime = table.Column<string>(type: "text", nullable: true),
                    TipGosta = table.Column<int>(type: "integer", nullable: false),
                    NazivFirme = table.Column<string>(type: "text", nullable: true),
                    KontaktOsoba = table.Column<string>(type: "text", nullable: true),
                    BrojTelefona = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Drzava = table.Column<string>(type: "text", nullable: true),
                    TipDokumenta = table.Column<string>(type: "text", nullable: false),
                    SlikaDokumenta = table.Column<string>(type: "text", nullable: false),
                    VIPGost = table.Column<bool>(type: "boolean", nullable: false),
                    Dodatno = table.Column<string>(type: "text", nullable: true),
                    OriginalniRecepcionerId = table.Column<int>(type: "integer", nullable: false),
                    OriginalnoVrijemeKreiranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OriginalnoVrijemeAzuriranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DatumArhiviranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArhiviraoRecepcionerId = table.Column<int>(type: "integer", nullable: false),
                    RazlogArhiviranja = table.Column<int>(type: "integer", nullable: false),
                    ArhiviranaRezervacijaId = table.Column<int>(type: "integer", nullable: true),
                    Napomena = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArhiviraniGosti", x => x.IdArhive);
                    table.ForeignKey(
                        name: "FK_ArhiviraniGosti_ArhiviraneRezervacije_ArhiviranaRezervacija~",
                        column: x => x.ArhiviranaRezervacijaId,
                        principalTable: "ArhiviraneRezervacije",
                        principalColumn: "IdArhive",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    IdRezervacije = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BrojRezervacije = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SobaId = table.Column<int>(type: "integer", nullable: false),
                    RecepcionerId = table.Column<int>(type: "integer", nullable: false),
                    DatumDolaska = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DatumOdlaska = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BrojOdraslih = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    BrojDjece = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CijenaPoNoci = table.Column<decimal>(type: "numeric", nullable: false),
                    Popust = table.Column<decimal>(type: "numeric", nullable: true),
                    UkupnaCijena = table.Column<decimal>(type: "numeric", nullable: false),
                    NacinRezervacije = table.Column<int>(type: "integer", nullable: false),
                    EksterniBrojRezervacije = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Zahtjevi = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Napomena = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    VrijemeKreiranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    VrijemeOtkazivanja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RazlogOtkazivanja = table.Column<string>(type: "text", nullable: true),
                    VrijemeAzuriranja = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.IdRezervacije);
                    table.CheckConstraint("CK_Rezervacija_Datumi", "\"DatumOdlaska\" > \"DatumDolaska\"");
                    table.CheckConstraint("CK_Rezervacija_Gosti", "\"BrojOdraslih\" + \"BrojDjece\" > 0");
                    table.ForeignKey(
                        name: "FK_Rezervacije_Recepcioneri_RecepcionerId",
                        column: x => x.RecepcionerId,
                        principalTable: "Recepcioneri",
                        principalColumn: "IdRecepcionera",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Sobe_SobaId",
                        column: x => x.SobaId,
                        principalTable: "Sobe",
                        principalColumn: "IdSobe",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RezervacijaGosti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RezervacijaId = table.Column<int>(type: "integer", nullable: false),
                    GostId = table.Column<int>(type: "integer", nullable: false),
                    JeGlavniGost = table.Column<bool>(type: "boolean", nullable: false),
                    PosebneNapomene = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RezervacijaGosti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RezervacijaGosti_Gosti_GostId",
                        column: x => x.GostId,
                        principalTable: "Gosti",
                        principalColumn: "IdGosta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RezervacijaGosti_Rezervacije_RezervacijaId",
                        column: x => x.RezervacijaId,
                        principalTable: "Rezervacije",
                        principalColumn: "IdRezervacije",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArhiviranaRezervacija_DatumArhiviranja",
                table: "ArhiviraneRezervacije",
                column: "DatumArhiviranja");

            migrationBuilder.CreateIndex(
                name: "IX_ArhiviranaRezervacija_OriginalniGostId",
                table: "ArhiviraneRezervacije",
                column: "OriginalniGostId");

            migrationBuilder.CreateIndex(
                name: "IX_ArhiviranaRezervacija_OriginalniId",
                table: "ArhiviraneRezervacije",
                column: "OriginalnaRezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_ArhiviraniGost_DatumArhiviranja",
                table: "ArhiviraniGosti",
                column: "DatumArhiviranja");

            migrationBuilder.CreateIndex(
                name: "IX_ArhiviraniGost_OriginalniId",
                table: "ArhiviraniGosti",
                column: "OriginalniGostId");

            migrationBuilder.CreateIndex(
                name: "IX_ArhiviraniGosti_ArhiviranaRezervacijaId",
                table: "ArhiviraniGosti",
                column: "ArhiviranaRezervacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gost_BrojTelefona",
                table: "Gosti",
                column: "BrojTelefona");

            migrationBuilder.CreateIndex(
                name: "IX_Gost_Email",
                table: "Gosti",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Logovi_Tip",
                table: "Logovi",
                column: "Tip");

            migrationBuilder.CreateIndex(
                name: "IX_Logovi_Vrijeme",
                table: "Logovi",
                column: "Vrijeme");

            migrationBuilder.CreateIndex(
                name: "IX_Recepcioner_Email",
                table: "Recepcioneri",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recepcioner_KorisnickoIme",
                table: "Recepcioneri",
                column: "KorisnickoIme",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaGosti_GostId",
                table: "RezervacijaGosti",
                column: "GostId");

            migrationBuilder.CreateIndex(
                name: "IX_RezervacijaGosti_RezervacijaId_GostId",
                table: "RezervacijaGosti",
                columns: new[] { "RezervacijaId", "GostId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_BrojRezervacije",
                table: "Rezervacije",
                column: "BrojRezervacije",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_Datumi",
                table: "Rezervacije",
                columns: new[] { "DatumDolaska", "DatumOdlaska" });

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_SobaId",
                table: "Rezervacije",
                column: "SobaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_Status",
                table: "Rezervacije",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_RecepcionerId",
                table: "Rezervacije",
                column: "RecepcionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Soba_BrojSobe",
                table: "Sobe",
                column: "BrojSobe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Soba_Status",
                table: "Sobe",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArhiviraniGosti");

            migrationBuilder.DropTable(
                name: "Logovi");

            migrationBuilder.DropTable(
                name: "RezervacijaGosti");

            migrationBuilder.DropTable(
                name: "ArhiviraneRezervacije");

            migrationBuilder.DropTable(
                name: "Gosti");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Recepcioneri");

            migrationBuilder.DropTable(
                name: "Sobe");
        }
    }
}
