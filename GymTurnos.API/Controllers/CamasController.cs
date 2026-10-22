using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymTurnos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CamasController : ControllerBase
{
    private readonly ICamaService _camaService;

    public CamasController(ICamaService camaService)
    {
        _camaService = camaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Cama>>> GetAll()
    {
        var camas = await _camaService.GetAllAsync();
        return Ok(camas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cama>> GetById(int id)
    {
        var cama = await _camaService.GetByIdAsync(id);

        if (cama == null)
            return NotFound();

        return Ok(cama);
    }

    [HttpPost]
    public async Task<ActionResult<Cama>> Create(Cama cama)
    {
        var nuevaCama = await _camaService.CreateAsync(cama);

        return CreatedAtAction(
            nameof(GetById),
            new { id = nuevaCama.Id },
            nuevaCama
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Cama>> Update(int id, Cama cama)
    {
        if (id != cama.Id)
            return BadRequest();

        var camaActualizada = await _camaService.UpdateAsync(cama);

        if (camaActualizada == null)
            return NotFound();

        return Ok(camaActualizada);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _camaService.DeleteAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}