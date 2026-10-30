using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        IPasswordHasher passwordHasher)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
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
        var usuarioExistente =
            await _usuarioRepository.GetByEmailAsync(usuario.Email);

        if (usuarioExistente != null)
        {
            throw new InvalidOperationException(
                "Ya existe un usuario registrado con ese email."
            );
        }

        if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
        {
            throw new InvalidOperationException(
                "La contraseña es obligatoria."
            );
        }

        usuario.PasswordHash =
            _passwordHasher.HashPassword(usuario.PasswordHash);

        usuario.Activo = true;

        return await _usuarioRepository.CreateAsync(usuario);
    }

    public async Task<Usuario?> UpdateAsync(Usuario usuario)
    {
        var usuarioActual = await _usuarioRepository.GetByIdAsync(usuario.Id);

        if (usuarioActual == null)
            return null;

        // Al modificar datos generales no cambiamos la contraseña.
        usuario.PasswordHash = usuarioActual.PasswordHash;

        return await _usuarioRepository.UpdateAsync(usuario);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _usuarioRepository.DeleteAsync(id);
    }
}