using System;
using System.Collections.Generic;
using System.Text;

namespace GymTurnos.Domain.Entities;

public class Cama
{
    public int Id { get; set; }

    public int Numero { get; set; }

    public bool Disponible { get; set; }
}