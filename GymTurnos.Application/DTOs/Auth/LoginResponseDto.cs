namespace GymTurnos.Application.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;

    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;
}