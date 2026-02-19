using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace receptionHubBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedICollections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecepcionerIdRecepcionera",
                table: "Rezervacije",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SobaIdSobe",
                table: "Rezervacije",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_RecepcionerIdRecepcionera",
                table: "Rezervacije",
                column: "RecepcionerIdRecepcionera");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_SobaIdSobe",
                table: "Rezervacije",
                column: "SobaIdSobe");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacije_Recepcioneri_RecepcionerIdRecepcionera",
                table: "Rezervacije",
                column: "RecepcionerIdRecepcionera",
                principalTable: "Recepcioneri",
                principalColumn: "IdRecepcionera");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacije_Sobe_SobaIdSobe",
                table: "Rezervacije",
                column: "SobaIdSobe",
                principalTable: "Sobe",
                principalColumn: "IdSobe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacije_Recepcioneri_RecepcionerIdRecepcionera",
                table: "Rezervacije");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacije_Sobe_SobaIdSobe",
                table: "Rezervacije");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacije_RecepcionerIdRecepcionera",
                table: "Rezervacije");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacije_SobaIdSobe",
                table: "Rezervacije");

            migrationBuilder.DropColumn(
                name: "RecepcionerIdRecepcionera",
                table: "Rezervacije");

            migrationBuilder.DropColumn(
                name: "SobaIdSobe",
                table: "Rezervacije");
        }
    }
}
