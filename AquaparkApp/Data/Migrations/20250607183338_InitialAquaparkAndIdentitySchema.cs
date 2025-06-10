using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaparkApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialAquaparkAndIdentitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atrakcja",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maxOsób = table.Column<int>(type: "int", nullable: false),
                    wymagaDodatkowejOplaty = table.Column<bool>(type: "bit", nullable: false),
                    cenaDodatkowa = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atrakcja", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AtrakcjaBramka",
                columns: table => new
                {
                    AtrakcjaId = table.Column<int>(type: "int", nullable: false),
                    BramkaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtrakcjaBramka", x => new { x.AtrakcjaId, x.BramkaId });
                });

            migrationBuilder.CreateTable(
                name: "Bramka",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    typBramki = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bramka", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Klient",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imię = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    nrTelefonu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "OfertaCennikowa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwaOferty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    typ = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cenaPodstawowa = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    limitCzasuMinuty = table.Column<int>(type: "int", nullable: true),
                    liczbaWejsc = table.Column<int>(type: "int", nullable: true),
                    karaZaMinutePrzekroczenia = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    obowiazujeOd = table.Column<DateTime>(type: "datetime", nullable: false),
                    obowiazujeDo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfertaCennikowa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Opaska",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numerOpaski = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    dataWydania = table.Column<DateTime>(type: "datetime", nullable: true),
                    dataWycofania = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opaska", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StatusWizyty",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusWizyty", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypKary",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    domyslnaKwota = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypKary", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Znizka",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kodZnizki = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    opis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    typZnizki = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    wartosc = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    czyAktywna = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Znizka", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DostepAtrakcjiBramka",
                columns: table => new
                {
                    bramka_id = table.Column<int>(type: "int", nullable: false),
                    atrakcja_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DostepAtrakcjiBramka", x => new { x.bramka_id, x.atrakcja_id });
                    table.ForeignKey(
                        name: "FK_DostepAtrakcjiBramka_Atrakcja",
                        column: x => x.atrakcja_id,
                        principalTable: "Atrakcja",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_DostepAtrakcjiBramka_Bramka",
                        column: x => x.bramka_id,
                        principalTable: "Bramka",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Platnosc",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    klient_id = table.Column<int>(type: "int", nullable: true),
                    kwotaCalkowita = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    dataPlatnosci = table.Column<DateTime>(type: "datetime", nullable: false),
                    metodaPlatnosci = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    statusPlatnosci = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platnosc", x => x.id);
                    table.ForeignKey(
                        name: "FK_Platnosc_Klient",
                        column: x => x.klient_id,
                        principalTable: "Klient",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ProduktZakupiony",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    klient_id = table.Column<int>(type: "int", nullable: false),
                    oferta_id = table.Column<int>(type: "int", nullable: false),
                    znizka_id = table.Column<int>(type: "int", nullable: true),
                    cenaZakupu = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    dataZakupu = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    pozostaloWejsc = table.Column<int>(type: "int", nullable: true),
                    waznyOd = table.Column<DateTime>(type: "datetime", nullable: true),
                    waznyDo = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProduktZakupiony", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProduktZakupiony_Klient",
                        column: x => x.klient_id,
                        principalTable: "Klient",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ProduktZakupiony_OfertaCennikowa",
                        column: x => x.oferta_id,
                        principalTable: "OfertaCennikowa",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ProduktZakupiony_Znizka",
                        column: x => x.znizka_id,
                        principalTable: "Znizka",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Wizyta",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    klient_id = table.Column<int>(type: "int", nullable: false),
                    opaska_id = table.Column<int>(type: "int", nullable: false),
                    produktZakupiony_id = table.Column<int>(type: "int", nullable: false),
                    czasWejscia = table.Column<DateTime>(type: "datetime", nullable: false),
                    czasWyjscia = table.Column<DateTime>(type: "datetime", nullable: true),
                    planowanyCzasWyjscia = table.Column<DateTime>(type: "datetime", nullable: true),
                    statusWizyty_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wizyta", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wizyta_Klient",
                        column: x => x.klient_id,
                        principalTable: "Klient",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Wizyta_Opaska",
                        column: x => x.opaska_id,
                        principalTable: "Opaska",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Wizyta_ProduktZakupiony",
                        column: x => x.produktZakupiony_id,
                        principalTable: "ProduktZakupiony",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Wizyta_StatusWizyty",
                        column: x => x.statusWizyty_id,
                        principalTable: "StatusWizyty",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Kara",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    wizyta_id = table.Column<int>(type: "int", nullable: false),
                    typKary_id = table.Column<int>(type: "int", nullable: false),
                    oferta_id = table.Column<int>(type: "int", nullable: true),
                    kwota = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    dataNaliczenia = table.Column<DateTime>(type: "datetime", nullable: false),
                    statusPlatnosci = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kara", x => x.id);
                    table.ForeignKey(
                        name: "FK_Kara_OfertaCennikowa",
                        column: x => x.oferta_id,
                        principalTable: "OfertaCennikowa",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Kara_TypKary",
                        column: x => x.typKary_id,
                        principalTable: "TypKary",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Kara_Wizyta",
                        column: x => x.wizyta_id,
                        principalTable: "Wizyta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "LogDostepu",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    wizyta_id = table.Column<int>(type: "int", nullable: false),
                    bramka_id = table.Column<int>(type: "int", nullable: false),
                    czasZdarzenia = table.Column<DateTime>(type: "datetime", nullable: false),
                    typZdarzenia = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogDostepu", x => x.id);
                    table.ForeignKey(
                        name: "FK_LogDostepu_Bramka",
                        column: x => x.bramka_id,
                        principalTable: "Bramka",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_LogDostepu_Wizyta",
                        column: x => x.wizyta_id,
                        principalTable: "Wizyta",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PozycjaPlatnosci",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    platnosc_id = table.Column<int>(type: "int", nullable: false),
                    produktZakupiony_id = table.Column<int>(type: "int", nullable: true),
                    kara_id = table.Column<int>(type: "int", nullable: true),
                    opisPozycji = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    kwotaPozycji = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PozycjaPlatnosci", x => x.id);
                    table.ForeignKey(
                        name: "FK_PozycjaPlatnosci_Kara",
                        column: x => x.kara_id,
                        principalTable: "Kara",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PozycjaPlatnosci_Platnosc",
                        column: x => x.platnosc_id,
                        principalTable: "Platnosc",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PozycjaPlatnosci_ProduktZakupiony",
                        column: x => x.produktZakupiony_id,
                        principalTable: "ProduktZakupiony",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DostepAtrakcjiBramka_atrakcja_id",
                table: "DostepAtrakcjiBramka",
                column: "atrakcja_id");

            migrationBuilder.CreateIndex(
                name: "IX_Kara_oferta_id",
                table: "Kara",
                column: "oferta_id");

            migrationBuilder.CreateIndex(
                name: "IX_Kara_typKary_id",
                table: "Kara",
                column: "typKary_id");

            migrationBuilder.CreateIndex(
                name: "IX_Kara_Wizyta",
                table: "Kara",
                column: "wizyta_id");

            migrationBuilder.CreateIndex(
                name: "IX_LogDostepu_Bramka",
                table: "LogDostepu",
                column: "bramka_id");

            migrationBuilder.CreateIndex(
                name: "IX_LogDostepu_Wizyta",
                table: "LogDostepu",
                column: "wizyta_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Opaska_NumerOpaski",
                table: "Opaska",
                column: "numerOpaski",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platnosc_Klient",
                table: "Platnosc",
                column: "klient_id",
                filter: "([klient_id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_PozycjaPlatnosci_Kara",
                table: "PozycjaPlatnosci",
                column: "kara_id",
                filter: "([kara_id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_PozycjaPlatnosci_Platnosc",
                table: "PozycjaPlatnosci",
                column: "platnosc_id");

            migrationBuilder.CreateIndex(
                name: "IX_PozycjaPlatnosci_Produkt",
                table: "PozycjaPlatnosci",
                column: "produktZakupiony_id",
                filter: "([produktZakupiony_id] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_ProduktZakupiony_Klient",
                table: "ProduktZakupiony",
                column: "klient_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProduktZakupiony_Oferta",
                table: "ProduktZakupiony",
                column: "oferta_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProduktZakupiony_znizka_id",
                table: "ProduktZakupiony",
                column: "znizka_id");

            migrationBuilder.CreateIndex(
                name: "UQ_StatusWizyty_Nazwa",
                table: "StatusWizyty",
                column: "nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_TypKary_Nazwa",
                table: "TypKary",
                column: "nazwa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wizyta_AktywnaOpaskaUnikalna",
                table: "Wizyta",
                column: "opaska_id",
                unique: true,
                filter: "[statusWizyty_id]=(1)");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyta_Klient",
                table: "Wizyta",
                column: "klient_id");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyta_Produkt",
                table: "Wizyta",
                column: "produktZakupiony_id");

            migrationBuilder.CreateIndex(
                name: "IX_Wizyta_statusWizyty_id",
                table: "Wizyta",
                column: "statusWizyty_id");

            migrationBuilder.CreateIndex(
                name: "UQ_Znizka_KodZnizki",
                table: "Znizka",
                column: "kodZnizki",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtrakcjaBramka");

            migrationBuilder.DropTable(
                name: "DostepAtrakcjiBramka");

            migrationBuilder.DropTable(
                name: "LogDostepu");

            migrationBuilder.DropTable(
                name: "PozycjaPlatnosci");

            migrationBuilder.DropTable(
                name: "Atrakcja");

            migrationBuilder.DropTable(
                name: "Bramka");

            migrationBuilder.DropTable(
                name: "Kara");

            migrationBuilder.DropTable(
                name: "Platnosc");

            migrationBuilder.DropTable(
                name: "TypKary");

            migrationBuilder.DropTable(
                name: "Wizyta");

            migrationBuilder.DropTable(
                name: "Opaska");

            migrationBuilder.DropTable(
                name: "ProduktZakupiony");

            migrationBuilder.DropTable(
                name: "StatusWizyty");

            migrationBuilder.DropTable(
                name: "Klient");

            migrationBuilder.DropTable(
                name: "OfertaCennikowa");

            migrationBuilder.DropTable(
                name: "Znizka");
        }
    }
}
