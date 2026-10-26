using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Services;

public interface IClaseService
{
    Task<List<Clase>> GetAllAsync();

    Task<Clase?> GetByIdAsync(int id);

    Task<Clase> CreateAsync(Clase clase);

    Task<Clase?> UpdateAsync(Clase clase);

    Task<bool> DeleteAsync(int id);
}