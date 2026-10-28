using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using GymTurnos.Domain.Enums;

namespace GymTurnos.Application.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IClaseRepository _claseRepository;
    private readonly ICamaRepository _camaRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public ReservaService(
        IReservaRepository reservaRepository,
        IClaseRepository claseRepository,
        ICamaRepository camaRepository,
        IUsuarioRepository usuarioRepository)
    {
        _reservaRepository = reservaRepository;
        _claseRepository = claseRepository;
        _camaRepository = camaRepository;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<Reserva>> GetAllAsync()
    {
        return await _reservaRepository.GetAllAsync();
    }

    public async Task<Reserva?> GetByIdAsync(int id)
    {
        return await _reservaRepository.GetByIdAsync(id);
    }

    public async Task<List<Reserva>> GetByClienteIdAsync(int clienteId)
    {
        return await _reservaRepository.GetByClienteIdAsync(clienteId);
    }

    public async Task<Reserva> CreateAsync(Reserva reserva)
    {
        var cliente = await _usuarioRepository.GetByIdAsync(reserva.ClienteId);

        if (cliente == null)
            throw new InvalidOperationException("El cliente no existe.");

        if (cliente.Rol != RolUsuario.Cliente)
            throw new InvalidOperationException(
                "El usuario seleccionado no tiene rol de cliente."
            );

        var clase = await _claseRepository.GetByIdAsync(reserva.ClaseId);

        if (clase == null)
            throw new InvalidOperationException("La clase no existe.");

        var cama = await _camaRepository.GetByIdAsync(reserva.CamaId);

        if (cama == null)
            throw new InvalidOperationException("La cama no existe.");

        if (!cama.Disponible)
            throw new InvalidOperationException(
                "La cama seleccionada no está disponible."
            );

        var camaOcupada =
            await _reservaRepository.ExisteReservaParaCamaYClaseAsync(
                reserva.CamaId,
                reserva.ClaseId
            );

        if (camaOcupada)
            throw new InvalidOperationException(
                "La cama ya está reservada para esta clase."
            );

        var reservasActuales =
            await _reservaRepository
                .ContarReservasActivasPorClaseAsync(reserva.ClaseId);

        if (reservasActuales >= clase.CupoMaximo)
            throw new InvalidOperationException(
                "La clase ya alcanzó el cupo máximo."
            );

        reserva.FechaReserva = DateTime.Now;
        reserva.Activa = true;

        return await _reservaRepository.CreateAsync(reserva);
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var reserva = await _reservaRepository.GetByIdAsync(id);

        if (reserva == null)
            return false;

        if (!reserva.Activa)
            throw new InvalidOperationException(
                "La reserva ya se encuentra cancelada."
            );

        reserva.Activa = false;

        await _reservaRepository.UpdateAsync(reserva);

        return true;
    }

    public async Task<Reserva?> UpdateAsync(Reserva reserva)
    {
        return await _reservaRepository.UpdateAsync(reserva);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _reservaRepository.DeleteAsync(id);
    }
}