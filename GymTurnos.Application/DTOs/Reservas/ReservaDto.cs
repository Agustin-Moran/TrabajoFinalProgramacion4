namespace GymTurnos.Application.DTOs.Reservas;

public class ReservaDto
{
    public int Id { get; set; }

    public DateTime FechaReserva { get; set; }

    public bool Activa { get; set; }

    public int ClienteId { get; set; }

    public string ClienteNombre { get; set; } = string.Empty;

    public int ClaseId { get; set; }

    public DateTime FechaHoraClase { get; set; }

    public int CamaId { get; set; }

    public int NumeroCama { get; set; }
}