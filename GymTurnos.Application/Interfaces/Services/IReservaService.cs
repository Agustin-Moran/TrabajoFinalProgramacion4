using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Services;

public interface IReservaService
{
    Task<List<Reserva>> GetAllAsync();

    Task<Reserva?> GetByIdAsync(int id);

    Task<List<Reserva>> GetByClienteIdAsync(int clienteId);

    Task<Reserva> CreateAsync(Reserva reserva);

    Task<bool> CancelarAsync(int id);

    Task<Reserva?> UpdateAsync(Reserva reserva);

    Task<bool> DeleteAsync(int id);
}