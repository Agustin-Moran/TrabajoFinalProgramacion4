using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<List<Usuario>> GetAllAsync();

    Task<Usuario?> GetByIdAsync(int id);

    Task<Usuario?> GetByEmailAsync(string email);

    Task<Usuario> CreateAsync(Usuario usuario);

    Task<Usuario?> UpdateAsync(Usuario usuario);

    Task<bool> DeleteAsync(int id);
}