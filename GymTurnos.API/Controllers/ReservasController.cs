using GymTurnos.Application.DTOs.Reservas;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;





namespace GymTurnos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReservasController : ControllerBase
{
    private readonly IReservaService _reservaService;

    public ReservasController(IReservaService reservaService)
    {
        _reservaService = reservaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReservaDto>>> GetAll()
    {
        var reservas = await _reservaService.GetAllAsync();

        var resultado = reservas.Select(r => new ReservaDto
        {
            Id = r.Id,
            FechaReserva = r.FechaReserva,
            Activa = r.Activa,
            ClienteId = r.ClienteId,
            ClienteNombre = r.Cliente != null
                ? $"{r.Cliente.Nombre} {r.Cliente.Apellido}"
                : string.Empty,
            ClaseId = r.ClaseId,
            FechaHoraClase = r.Clase?.FechaHora ?? default,
            CamaId = r.CamaId,
            NumeroCama = r.Cama?.Numero ?? 0
        }).ToList();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservaDto>> GetById(int id)
    {
        var r = await _reservaService.GetByIdAsync(id);

        if (r == null)
            return NotFound();

        return Ok(new ReservaDto
        {
            Id = r.Id,
            FechaReserva = r.FechaReserva,
            Activa = r.Activa,
            ClienteId = r.ClienteId,
            ClienteNombre = r.Cliente != null
                ? $"{r.Cliente.Nombre} {r.Cliente.Apellido}"
                : string.Empty,
            ClaseId = r.ClaseId,
            FechaHoraClase = r.Clase?.FechaHora ?? default,
            CamaId = r.CamaId,
            NumeroCama = r.Cama?.Numero ?? 0
        });
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<List<ReservaDto>>> GetByCliente(int clienteId)
    {
        var reservas = await _reservaService.GetByClienteIdAsync(clienteId);

        var resultado = reservas.Select(r => new ReservaDto
        {
            Id = r.Id,
            FechaReserva = r.FechaReserva,
            Activa = r.Activa,
            ClienteId = r.ClienteId,
            ClienteNombre = r.Cliente != null
                ? $"{r.Cliente.Nombre} {r.Cliente.Apellido}"
                : string.Empty,
            ClaseId = r.ClaseId,
            FechaHoraClase = r.Clase?.FechaHora ?? default,
            CamaId = r.CamaId,
            NumeroCama = r.Cama?.Numero ?? 0
        }).ToList();

        return Ok(resultado);
    }

    [HttpGet("mis-reservas")]
    [Authorize(Roles = "Cliente")]
    public async Task<ActionResult<List<ReservaDto>>> GetMisReservas()
    {
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (usuarioIdClaim == null)
            return Unauthorized();

        var clienteId = int.Parse(usuarioIdClaim.Value);

        var reservas =
            await _reservaService.GetByClienteIdAsync(clienteId);

        var resultado = reservas.Select(r => new ReservaDto
        {
            Id = r.Id,
            FechaReserva = r.FechaReserva,
            Activa = r.Activa,
            ClienteId = r.ClienteId,

            ClienteNombre = r.Cliente != null
                ? $"{r.Cliente.Nombre} {r.Cliente.Apellido}"
                : string.Empty,

            ClaseId = r.ClaseId,

            FechaHoraClase =
                r.Clase?.FechaHora ?? default,

            CamaId = r.CamaId,

            NumeroCama =
                r.Cama?.Numero ?? 0
        }).ToList();

        return Ok(resultado);
    }

    [HttpPost]
    public async Task<ActionResult<ReservaDto>> Create(ReservaCreateDto dto)
    {
        try
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (usuarioIdClaim == null)
                return Unauthorized();

            var clienteId = int.Parse(usuarioIdClaim.Value);

            var reserva = new Reserva
            {
                ClienteId = clienteId,
                ClaseId = dto.ClaseId,
                CamaId = dto.CamaId
            };

            var nueva = await _reservaService.CreateAsync(reserva);

            return CreatedAtAction(
                nameof(GetById),
                new { id = nueva.Id },
                new ReservaDto
                {
                    Id = nueva.Id,
                    FechaReserva = nueva.FechaReserva,
                    Activa = nueva.Activa,
                    ClienteId = nueva.ClienteId,
                    ClaseId = nueva.ClaseId,
                    CamaId = nueva.CamaId
                }
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        try
        {
            var resultado = await _reservaService.CancelarAsync(id);

            if (!resultado)
                return NotFound();

            return Ok(new
            {
                mensaje = "Reserva cancelada correctamente."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _reservaService.DeleteAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }
}