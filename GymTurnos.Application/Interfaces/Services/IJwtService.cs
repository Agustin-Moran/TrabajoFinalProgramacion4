using GymTurnos.Domain.Entities;

namespace GymTurnos.Application.Interfaces.Services;

public interface IJwtService
{
    string GenerarToken(Usuario usuario);
}