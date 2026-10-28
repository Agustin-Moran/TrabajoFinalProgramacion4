using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Repositories;

public interface IReservaRepository
{
    Task<List<Reserva>> GetAllAsync();

    Task<Reserva?> GetByIdAsync(int id);

    Task<List<Reserva>> GetByClienteIdAsync(int clienteId);

    Task<int> ContarReservasActivasPorClaseAsync(int claseId);

    Task<bool> ExisteReservaParaCamaYClaseAsync(int camaId, int claseId);

    Task<Reserva> CreateAsync(Reserva reserva);

    Task<Reserva?> UpdateAsync(Reserva reserva);

    Task<bool> DeleteAsync(int id);
}