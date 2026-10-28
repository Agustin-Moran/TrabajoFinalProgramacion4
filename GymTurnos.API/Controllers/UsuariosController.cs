using GymTurnos.Application.DTOs.Usuarios;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;




namespace GymTurnos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioDto>>> GetAll()
    {
        var usuarios = await _usuarioService.GetAllAsync();

        var resultado = usuarios.Select(u => new UsuarioDto
        {
            Id = u.Id,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            Email = u.Email,
            Activo = u.Activo,
            Rol = u.Rol
        }).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetById(int id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);

        if (usuario == null)
            return NotFound();

        var resultado = new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Activo = usuario.Activo,
            Rol = usuario.Rol
        };

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Create(UsuarioCreateDto dto)
    {
        try
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                PasswordHash = dto.Password,
                Rol = dto.Rol
            };

            var nuevoUsuario = await _usuarioService.CreateAsync(usuario);

            var resultado = new UsuarioDto
            {
                Id = nuevoUsuario.Id,
                Nombre = nuevoUsuario.Nombre,
                Apellido = nuevoUsuario.Apellido,
                Email = nuevoUsuario.Email,
                Activo = nuevoUsuario.Activo,
                Rol = nuevoUsuario.Rol
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = nuevoUsuario.Id },
                resultado
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UsuarioDto>> Update(
        int id,
        UsuarioUpdateDto dto)
    {
        var usuarioExistente = await _usuarioService.GetByIdAsync(id);

        if (usuarioExistente == null)
            return NotFound();

        usuarioExistente.Nombre = dto.Nombre;
        usuarioExistente.Apellido = dto.Apellido;
        usuarioExistente.Email = dto.Email;
        usuarioExistente.Activo = dto.Activo;
        usuarioExistente.Rol = dto.Rol;

        try
        {
            var usuarioActualizado =
                await _usuarioService.UpdateAsync(usuarioExistente);

            if (usuarioActualizado == null)
                return NotFound();

            var resultado = new UsuarioDto
            {
                Id = usuarioActualizado.Id,
                Nombre = usuarioActualizado.Nombre,
                Apellido = usuarioActualizado.Apellido,
                Email = usuarioActualizado.Email,
                Activo = usuarioActualizado.Activo,
                Rol = usuarioActualizado.Rol
            };

            return Ok(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                mensaje = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _usuarioService.DeleteAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}