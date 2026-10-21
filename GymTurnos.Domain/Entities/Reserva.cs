using System;
using System.Collections.Generic;
using System.Text;

namespace GymTurnos.Domain.Entities;

public class Reserva
{
    public int Id { get; set; }

    public DateTime FechaReserva { get; set; }

    public bool Activa { get; set; } = true;

    public int ClienteId { get; set; }

    public Usuario? Cliente { get; set; }

    public int ClaseId { get; set; }

    public Clase? Clase { get; set; }

    public int CamaId { get; set; }

    public Cama? Cama { get; set; }
}