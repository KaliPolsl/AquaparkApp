using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaparkApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOpisToLogDostepu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "typZdarzenia",
                table: "LogDostepu",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "opis",
                table: "LogDostepu",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "opis",
                table: "LogDostepu");

            migrationBuilder.AlterColumn<string>(
                name: "typZdarzenia",
                table: "LogDostepu",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
