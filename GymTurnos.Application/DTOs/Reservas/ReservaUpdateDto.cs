namespace GymTurnos.Application.DTOs.Reservas;

public class ReservaUpdateDto
{
    public bool Activa { get; set; }

    public int ClienteId { get; set; }

    public int ClaseId { get; set; }

    public int CamaId { get; set; }
}