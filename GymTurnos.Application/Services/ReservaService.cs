using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Application.Interfaces.Services;
using GymTurnos.Domain.Entities;
using GymTurnos.Domain.Enums;

namespace GymTurnos.Application.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IClaseRepository _claseRepository;
    private readonly ICamaRepository _camaRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmailService _emailService;

    public ReservaService(
        IReservaRepository reservaRepository,
        IClaseRepository claseRepository,
        ICamaRepository camaRepository,
        IUsuarioRepository usuarioRepository,
        IEmailService emailService)
    {
        _reservaRepository = reservaRepository;
        _claseRepository = claseRepository;
        _camaRepository = camaRepository;
        _usuarioRepository = usuarioRepository;
        _emailService = emailService;
    }

    public async Task<List<Reserva>> GetAllAsync()
    {
        return await _reservaRepository.GetAllAsync();
    }

    public async Task<Reserva?> GetByIdAsync(int id)
    {
        return await _reservaRepository.GetByIdAsync(id);
    }

    public async Task<List<Reserva>> GetByClienteIdAsync(int clienteId)
    {
        return await _reservaRepository.GetByClienteIdAsync(clienteId);
    }

    public async Task<Reserva> CreateAsync(Reserva reserva)
    {
        var cliente =
            await _usuarioRepository.GetByIdAsync(reserva.ClienteId);

        if (cliente == null)
            throw new InvalidOperationException(
                "El cliente no existe."
            );

        if (cliente.Rol != RolUsuario.Cliente)
            throw new InvalidOperationException(
                "El usuario seleccionado no tiene rol de cliente."
            );

        if (!cliente.Activo)
            throw new InvalidOperationException(
                "El cliente se encuentra inactivo."
            );

        var clase =
            await _claseRepository.GetByIdAsync(reserva.ClaseId);

        if (clase == null)
            throw new InvalidOperationException(
                "La clase no existe."
            );

        var cama =
            await _camaRepository.GetByIdAsync(reserva.CamaId);

        if (cama == null)
            throw new InvalidOperationException(
                "La cama no existe."
            );

        if (!cama.Disponible)
            throw new InvalidOperationException(
                "La cama seleccionada no está disponible."
            );

        var camaOcupada =
            await _reservaRepository
                .ExisteReservaParaCamaYClaseAsync(
                reserva.CamaId,
                reserva.ClaseId
            );

        if (camaOcupada)
            throw new InvalidOperationException(
                "La cama ya está reservada para esta clase."
            );

        var reservasActuales =
            await _reservaRepository
                .ContarReservasActivasPorClaseAsync(
                    reserva.ClaseId
                );

        if (reservasActuales >= clase.CupoMaximo)
            throw new InvalidOperationException(
                "La clase ya alcanzó el cupo máximo."
            );

        reserva.FechaReserva = DateTime.Now;
        reserva.Activa = true;

        var nuevaReserva =
            await _reservaRepository.CreateAsync(reserva);

        var html = $"""
            <h2>Reserva confirmada</h2>

            <p>Hola {cliente.Nombre},</p>

            <p>
                Tu turno de Pilates fue reservado correctamente.
            </p>

            <p>
                <strong>Fecha:</strong>
                {clase.FechaHora:dd/MM/yyyy}
            </p>

            <p>
                <strong>Hora:</strong>
                {clase.FechaHora:HH:mm}
            </p>

            <p>
                <strong>Cama:</strong>
                {cama.Numero}
            </p>

            <p>¡Te esperamos!</p>

            <p>GymTurnos</p>
            """;

        try
        {
            await _emailService.EnviarEmailAsync(
                cliente.Email,
                "Reserva confirmada - GymTurnos",
                html,
                $"reserva-confirmada-{nuevaReserva.Id}"
            );
        }
        catch
        {
            // La reserva ya fue creada.
            // Si falla Resend, no cancelamos la operación principal.
        }

        return nuevaReserva;
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var reserva =
            await _reservaRepository.GetByIdAsync(id);

        if (reserva == null)
            return false;

        if (!reserva.Activa)
            throw new InvalidOperationException(
                "La reserva ya se encuentra cancelada."
            );

        reserva.Activa = false;

        await _reservaRepository.UpdateAsync(reserva);

        if (reserva.Cliente != null)
        {
            var html = $"""
                <h2>Reserva cancelada</h2>

                <p>
                    Hola {reserva.Cliente.Nombre},
                </p>

                <p>
                    Tu reserva fue cancelada correctamente.
                </p>

                <p>
                    <strong>Número de reserva:</strong>
                    {reserva.Id}
                </p>

                <p>GymTurnos</p>
                """;

            try
            {
                await _emailService.EnviarEmailAsync(
                    reserva.Cliente.Email,
                    "Reserva cancelada - GymTurnos",
                    html,
                    $"reserva-cancelada-{reserva.Id}"
                );
            }
            catch
            {
                // La cancelación queda guardada aunque falle el email.
            }
        }

        return true;
    }

    public async Task<Reserva?> UpdateAsync(
        Reserva reserva)
    {
        return await _reservaRepository.UpdateAsync(reserva);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _reservaRepository.DeleteAsync(id);
    }
}