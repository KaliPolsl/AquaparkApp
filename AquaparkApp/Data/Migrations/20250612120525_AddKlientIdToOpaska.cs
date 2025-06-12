using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaparkApp.Migrations
{
    /// <inheritdoc />
    public partial class AddKlientIdToOpaska : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "klientId",
                table: "Opaska",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Opaska_klientId",
                table: "Opaska",
                column: "klientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Opaska_Klient_klientId",
                table: "Opaska",
                column: "klientId",
                principalTable: "Klient",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opaska_Klient_klientId",
                table: "Opaska");

            migrationBuilder.DropIndex(
                name: "IX_Opaska_klientId",
                table: "Opaska");

            migrationBuilder.DropColumn(
                name: "klientId",
                table: "Opaska");
        }
    }
}
