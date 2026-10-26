using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Domain.Entities;
using GymTurnos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymTurnos.Infrastructure.Repositories;

public class ClaseRepository : IClaseRepository
{
    private readonly AppDbContext _context;

    public ClaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Clase>> GetAllAsync()
    {
        return await _context.Clases
            .Include(c => c.Profesor)
            .ToListAsync();
    }

    public async Task<Clase?> GetByIdAsync(int id)
    {
        return await _context.Clases
            .Include(c => c.Profesor)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Clase> CreateAsync(Clase clase)
    {
        _context.Clases.Add(clase);
        await _context.SaveChangesAsync();

        return clase;
    }

    public async Task<Clase?> UpdateAsync(Clase clase)
    {
        var claseExistente = await _context.Clases.FindAsync(clase.Id);

        if (claseExistente == null)
            return null;

        claseExistente.FechaHora = clase.FechaHora;
        claseExistente.CupoMaximo = clase.CupoMaximo;
        claseExistente.ProfesorId = clase.ProfesorId;

        await _context.SaveChangesAsync();

        return claseExistente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var clase = await _context.Clases.FindAsync(id);

        if (clase == null)
            return false;

        _context.Clases.Remove(clase);
        await _context.SaveChangesAsync();

        return true;
    }
}