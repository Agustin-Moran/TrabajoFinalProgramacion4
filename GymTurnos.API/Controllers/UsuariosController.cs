using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymTurnos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> GetAll()
    {
        var usuarios = await _usuarioService.GetAllAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetById(int id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);

        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> Create(Usuario usuario)
    {
        var nuevoUsuario = await _usuarioService.CreateAsync(usuario);

        return CreatedAtAction(
            nameof(GetById),
            new { id = nuevoUsuario.Id },
            nuevoUsuario
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Usuario>> Update(int id, Usuario usuario)
    {
        if (id != usuario.Id)
            return BadRequest();

        var usuarioActualizado = await _usuarioService.UpdateAsync(usuario);

        if (usuarioActualizado == null)
            return NotFound();

        return Ok(usuarioActualizado);
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