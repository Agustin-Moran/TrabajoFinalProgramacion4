using GymTurnos.Domain.Enums;

namespace GymTurnos.Application.DTOs.Usuarios;

public class UsuarioUpdateDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public RolUsuario Rol { get; set; }
}