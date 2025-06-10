namespace AquaparkApp.Models.DTOs;

public class PozycjaKoszykaDto
{
    public int OfertaId { get; set; }
    public int Ilosc { get; set; } = 1; // Domyślnie 1
    // Można tu dodać więcej pól, jeśli będą potrzebne
}