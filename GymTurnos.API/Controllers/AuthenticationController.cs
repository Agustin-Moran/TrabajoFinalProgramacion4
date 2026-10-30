using GymTurnos.Application.DTOs.Auth;
using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymTurnos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthenticationController(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        LoginDto dto)
    {
        var usuario =
            await _usuarioRepository.GetByEmailAsync(dto.Email);

        if (usuario == null)
        {
            return Unauthorized(new
            {
                mensaje = "Email o contraseña incorrectos."
            });
        }

        if (!usuario.Activo)
        {
            return Unauthorized(new
            {
                mensaje = "El usuario se encuentra inactivo."
            });
        }

        var passwordCorrecta =
            _passwordHasher.VerifyPassword(
                dto.Password,
                usuario.PasswordHash
            );

        if (!passwordCorrecta)
        {
            return Unauthorized(new
            {
                mensaje = "Email o contraseña incorrectos."
            });
        }

        var token = _jwtService.GenerarToken(usuario);

        return Ok(new LoginResponseDto
        {
            Token = token,
            UsuarioId = usuario.Id,
            Nombre = $"{usuario.Nombre} {usuario.Apellido}",
            Rol = usuario.Rol.ToString()
        });
    }
}