using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaparkApp.Migrations
{
    /// <inheritdoc />
    public partial class malaMigracja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "typKaryPrzekroczenia_id",
                table: "OfertaCennikowa",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfertaCennikowa_typKaryPrzekroczenia_id",
                table: "OfertaCennikowa",
                column: "typKaryPrzekroczenia_id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfertaCennikowa_TypKary_typKaryPrzekroczenia_id",
                table: "OfertaCennikowa",
                column: "typKaryPrzekroczenia_id",
                principalTable: "TypKary",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfertaCennikowa_TypKary_typKaryPrzekroczenia_id",
                table: "OfertaCennikowa");

            migrationBuilder.DropIndex(
                name: "IX_OfertaCennikowa_typKaryPrzekroczenia_id",
                table: "OfertaCennikowa");

            migrationBuilder.DropColumn(
                name: "typKaryPrzekroczenia_id",
                table: "OfertaCennikowa");
        }
    }
}
