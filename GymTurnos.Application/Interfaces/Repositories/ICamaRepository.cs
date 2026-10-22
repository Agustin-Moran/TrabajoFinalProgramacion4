using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Repositories;

public interface ICamaRepository
{
    Task<List<Cama>> GetAllAsync();

    Task<Cama?> GetByIdAsync(int id);

    Task<Cama> CreateAsync(Cama cama);

    Task<Cama?> UpdateAsync(Cama cama);

    Task<bool> DeleteAsync(int id);
}