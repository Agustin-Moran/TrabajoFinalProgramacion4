using System;
using System.Collections.Generic;
using System.Text;
using GymTurnos.Domain.Enums;

namespace GymTurnos.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public Rol Rol { get; set; }
    public bool Activo { get; set; } = true;
}