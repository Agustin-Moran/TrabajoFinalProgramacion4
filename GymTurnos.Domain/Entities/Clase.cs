using System;
using System.Collections.Generic;
using System.Text;

namespace GymTurnos.Domain.Entities;

public class Clase
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public int CupoMaximo { get; set; }

    public int ProfesorId { get; set; }

    public Profesor? Profesor { get; set; }
}