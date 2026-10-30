using GymTurnos.Domain.Enums;

namespace GymTurnos.Application.DTOs.Usuarios;

public class UsuarioDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public RolUsuario Rol { get; set; }
}