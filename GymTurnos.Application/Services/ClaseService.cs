using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Services;

public class ClaseService : IClaseService
{
    private readonly IClaseRepository _claseRepository;

    public ClaseService(IClaseRepository claseRepository)
    {
        _claseRepository = claseRepository;
    }

    public async Task<List<Clase>> GetAllAsync()
    {
        return await _claseRepository.GetAllAsync();
    }

    public async Task<Clase?> GetByIdAsync(int id)
    {
        return await _claseRepository.GetByIdAsync(id);
    }

    public async Task<Clase> CreateAsync(Clase clase)
    {
        return await _claseRepository.CreateAsync(clase);
    }

    public async Task<Clase?> UpdateAsync(Clase clase)
    {
        return await _claseRepository.UpdateAsync(clase);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _claseRepository.DeleteAsync(id);
    }
}