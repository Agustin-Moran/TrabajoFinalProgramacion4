using GymTurnos.Domain.Enums;

namespace GymTurnos.Application.DTOs.Usuarios;

public class UsuarioCreateDto
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public RolUsuario Rol { get; set; }
}