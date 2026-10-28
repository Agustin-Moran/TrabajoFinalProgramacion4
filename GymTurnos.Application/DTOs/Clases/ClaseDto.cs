namespace GymTurnos.Application.DTOs.Clases;

public class ClaseDto
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public int CupoMaximo { get; set; }

    public int ProfesorId { get; set; }

    public string ProfesorNombre { get; set; } = string.Empty;
}