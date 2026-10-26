using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _usuarioRepository.GetAllAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        return await _usuarioRepository.CreateAsync(usuario);
    }

    public async Task<Usuario?> UpdateAsync(Usuario usuario)
    {
        return await _usuarioRepository.UpdateAsync(usuario);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _usuarioRepository.DeleteAsync(id);
    }
}