using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;

    public ReservaService(IReservaRepository reservaRepository)
    {
        _reservaRepository = reservaRepository;
    }

    public async Task<List<Reserva>> GetAllAsync()
    {
        return await _reservaRepository.GetAllAsync();
    }

    public async Task<Reserva?> GetByIdAsync(int id)
    {
        return await _reservaRepository.GetByIdAsync(id);
    }

    public async Task<Reserva> CreateAsync(Reserva reserva)
    {
        var existeReserva =
            await _reservaRepository.ExisteReservaParaCamaYClaseAsync(
                reserva.CamaId,
                reserva.ClaseId
            );

        if (existeReserva)
        {
            throw new InvalidOperationException(
                "La cama seleccionada ya está reservada para esa clase."
            );
        }

        return await _reservaRepository.CreateAsync(reserva);
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