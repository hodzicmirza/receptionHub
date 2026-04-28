using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace receptionHubBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Recepcioneri",
                columns: new[] { "IdRecepcionera", "Aktivan", "BrojTelefona", "DatumKreiranjaRacuna", "Email", "Ime", "KorisnickoIme", "LozinkaHash", "Napomena", "PosljednjiLogin", "Pozicija", "Prezime", "SlikaProfila" },
                values: new object[] { 1, true, null, new DateTime(2026, 3, 1, 17, 29, 24, 702, DateTimeKind.Utc).AddTicks(3284), "mhodzic6@etf.unsa.ba", "Mirza", "mirzah", "AQAAAAIAAYagAAAAEG855xKLPevH43KTI26jqvHcnP+W2xW1aEhcN6V9s5rbRrxZYvVeEhDY1WFlkPVOvA==", "Sistemski admin", null, 1, "hodzic", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recepcioneri",
                keyColumn: "IdRecepcionera",
                keyValue: 1);
        }
    }
}
