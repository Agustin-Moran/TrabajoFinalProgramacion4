using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Services;

public interface IUsuarioService
{
    Task<List<Usuario>> GetAllAsync();

    Task<Usuario?> GetByIdAsync(int id);

    Task<Usuario> CreateAsync(Usuario usuario);

    Task<Usuario?> UpdateAsync(Usuario usuario);

    Task<bool> DeleteAsync(int id);
}