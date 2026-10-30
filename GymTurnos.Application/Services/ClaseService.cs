using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using GymTurnos.Domain.Enums;

namespace GymTurnos.Application.Services;

public class ClaseService : IClaseService
{
    private readonly IClaseRepository _claseRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public ClaseService(
        IClaseRepository claseRepository,
        IUsuarioRepository usuarioRepository)
    {
        _claseRepository = claseRepository;
        _usuarioRepository = usuarioRepository;
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
        var profesor = await _usuarioRepository.GetByIdAsync(clase.ProfesorId);

        if (profesor == null)
            throw new InvalidOperationException("El profesor no existe.");

        if (profesor.Rol != RolUsuario.Profesor)
            throw new InvalidOperationException(
                "El usuario seleccionado no tiene rol de profesor."
            );

        if (!profesor.Activo)
            throw new InvalidOperationException(
                "El profesor seleccionado se encuentra inactivo."
            );

        if (clase.CupoMaximo <= 0)
            throw new InvalidOperationException(
                "El cupo máximo debe ser mayor a cero."
            );

        return await _claseRepository.CreateAsync(clase);
    }

    public async Task<Clase?> UpdateAsync(Clase clase)
    {
        var profesor = await _usuarioRepository.GetByIdAsync(clase.ProfesorId);

        if (profesor == null)
            throw new InvalidOperationException("El profesor no existe.");

        if (profesor.Rol != RolUsuario.Profesor)
            throw new InvalidOperationException(
                "El usuario seleccionado no tiene rol de profesor."
            );

        if (!profesor.Activo)
            throw new InvalidOperationException(
                "El profesor seleccionado se encuentra inactivo."
            );

        if (clase.CupoMaximo <= 0)
            throw new InvalidOperationException(
                "El cupo máximo debe ser mayor a cero."
            );

        return await _claseRepository.UpdateAsync(clase);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _claseRepository.DeleteAsync(id);
    }
}