using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Repositories;

public interface IClaseRepository
{
    Task<List<Clase>> GetAllAsync();

    Task<Clase?> GetByIdAsync(int id);

    Task<Clase> CreateAsync(Clase clase);

    Task<Clase?> UpdateAsync(Clase clase);

    Task<bool> DeleteAsync(int id);
}