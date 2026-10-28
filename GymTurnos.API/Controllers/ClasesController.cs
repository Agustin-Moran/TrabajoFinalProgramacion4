using GymTurnos.Application.DTOs.Clases;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;






namespace GymTurnos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClasesController : ControllerBase
{
    private readonly IClaseService _claseService;

    public ClasesController(IClaseService claseService)
    {
        _claseService = claseService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ClaseDto>>> GetAll()
    {
        var clases = await _claseService.GetAllAsync();

        var resultado = clases.Select(c => new ClaseDto
        {
            Id = c.Id,
            FechaHora = c.FechaHora,
            CupoMaximo = c.CupoMaximo,
            ProfesorId = c.ProfesorId,
            ProfesorNombre = c.Profesor != null
                ? $"{c.Profesor.Nombre} {c.Profesor.Apellido}"
                : string.Empty
        }).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClaseDto>> GetById(int id)
    {
        var clase = await _claseService.GetByIdAsync(id);

        if (clase == null)
            return NotFound();

        return Ok(new ClaseDto
        {
            Id = clase.Id,
            FechaHora = clase.FechaHora,
            CupoMaximo = clase.CupoMaximo,
            ProfesorId = clase.ProfesorId,
            ProfesorNombre = clase.Profesor != null
                ? $"{clase.Profesor.Nombre} {clase.Profesor.Apellido}"
                : string.Empty
        });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ClaseDto>> Create(ClaseCreateDto dto)
    {
        try
        {
            var clase = new Clase
            {
                FechaHora = dto.FechaHora,
                CupoMaximo = dto.CupoMaximo,
                ProfesorId = dto.ProfesorId
            };

            var nuevaClase = await _claseService.CreateAsync(clase);

            return CreatedAtAction(
                nameof(GetById),
                new { id = nuevaClase.Id },
                new ClaseDto
                {
                    Id = nuevaClase.Id,
                    FechaHora = nuevaClase.FechaHora,
                    CupoMaximo = nuevaClase.CupoMaximo,
                    ProfesorId = nuevaClase.ProfesorId
                }
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ClaseDto>> Update(
        int id,
        ClaseUpdateDto dto)
    {
        var claseExistente = await _claseService.GetByIdAsync(id);

        if (claseExistente == null)
            return NotFound();

        claseExistente.FechaHora = dto.FechaHora;
        claseExistente.CupoMaximo = dto.CupoMaximo;
        claseExistente.ProfesorId = dto.ProfesorId;

        try
        {
            var actualizada = await _claseService.UpdateAsync(claseExistente);

            if (actualizada == null)
                return NotFound();

            return Ok(new ClaseDto
            {
                Id = actualizada.Id,
                FechaHora = actualizada.FechaHora,
                CupoMaximo = actualizada.CupoMaximo,
                ProfesorId = actualizada.ProfesorId
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _claseService.DeleteAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}