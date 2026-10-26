using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Services;

public interface IReservaService
{
    Task<List<Reserva>> GetAllAsync();

    Task<Reserva?> GetByIdAsync(int id);

    Task<Reserva> CreateAsync(Reserva reserva);

    Task<Reserva?> UpdateAsync(Reserva reserva);

    Task<bool> DeleteAsync(int id);
}