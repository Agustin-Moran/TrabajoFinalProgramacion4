using GymTurnos.Application.DTOs.Camas;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GymTurnos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CamasController : ControllerBase
{
    private readonly ICamaService _camaService;

    public CamasController(ICamaService camaService)
    {
        _camaService = camaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CamaDto>>> GetAll()
    {
        var camas = await _camaService.GetAllAsync();

        var resultado = camas.Select(c => new CamaDto
        {
            Id = c.Id,
            Numero = c.Numero,
            Disponible = c.Disponible
        }).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CamaDto>> GetById(int id)
    {
        var cama = await _camaService.GetByIdAsync(id);

        if (cama == null)
            return NotFound();

        return Ok(new CamaDto
        {
            Id = cama.Id,
            Numero = cama.Numero,
            Disponible = cama.Disponible
        });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<CamaDto>> Create(CamaCreateDto dto)
    {
        var cama = new Cama
        {
            Numero = dto.Numero,
            Disponible = dto.Disponible
        };

        var nuevaCama = await _camaService.CreateAsync(cama);

        var resultado = new CamaDto
        {
            Id = nuevaCama.Id,
            Numero = nuevaCama.Numero,
            Disponible = nuevaCama.Disponible
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = nuevaCama.Id },
            resultado
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<CamaDto>> Update(int id, CamaUpdateDto dto)
    {
        var camaExistente = await _camaService.GetByIdAsync(id);

        if (camaExistente == null)
            return NotFound();

        camaExistente.Numero = dto.Numero;
        camaExistente.Disponible = dto.Disponible;

        var camaActualizada = await _camaService.UpdateAsync(camaExistente);

        if (camaActualizada == null)
            return NotFound();

        return Ok(new CamaDto
        {
            Id = camaActualizada.Id,
            Numero = camaActualizada.Numero,
            Disponible = camaActualizada.Disponible
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _camaService.DeleteAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}