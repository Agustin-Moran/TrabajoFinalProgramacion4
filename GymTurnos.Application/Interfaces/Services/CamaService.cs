using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Services;

public class CamaService : ICamaService
{
    private readonly ICamaRepository _camaRepository;

    public CamaService(ICamaRepository camaRepository)
    {
        _camaRepository = camaRepository;
    }

    public async Task<List<Cama>> GetAllAsync()
    {
        return await _camaRepository.GetAllAsync();
    }

    public async Task<Cama?> GetByIdAsync(int id)
    {
        return await _camaRepository.GetByIdAsync(id);
    }

    public async Task<Cama> CreateAsync(Cama cama)
    {
        return await _camaRepository.CreateAsync(cama);
    }

    public async Task<Cama?> UpdateAsync(Cama cama)
    {
        return await _camaRepository.UpdateAsync(cama);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _camaRepository.DeleteAsync(id);
    }
}