using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Domain.Entities;
using GymTurnos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymTurnos.Infrastructure.Repositories;

public class ReservaRepository : IReservaRepository
{
    private readonly AppDbContext _context;

    public ReservaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reserva>> GetAllAsync()
    {
        return await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Clase)
            .Include(r => r.Cama)
            .ToListAsync();
    }

    public async Task<Reserva?> GetByIdAsync(int id)
    {
        return await _context.Reservas
            .Include(r => r.Cliente)
            .Include(r => r.Clase)
            .Include(r => r.Cama)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Reserva> CreateAsync(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return reserva;
    }

    public async Task<Reserva?> UpdateAsync(Reserva reserva)
    {
        var reservaExistente = await _context.Reservas.FindAsync(reserva.Id);

        if (reservaExistente == null)
            return null;

        reservaExistente.FechaReserva = reserva.FechaReserva;
        reservaExistente.Activa = reserva.Activa;
        reservaExistente.ClienteId = reserva.ClienteId;
        reservaExistente.ClaseId = reserva.ClaseId;
        reservaExistente.CamaId = reserva.CamaId;

        await _context.SaveChangesAsync();

        return reservaExistente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);

        if (reserva == null)
            return false;

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExisteReservaParaCamaYClaseAsync(
        int camaId,
        int claseId)
    {
        return await _context.Reservas.AnyAsync(r =>
            r.CamaId == camaId &&
            r.ClaseId == claseId &&
            r.Activa);
    }
}