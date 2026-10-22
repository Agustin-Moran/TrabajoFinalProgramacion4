using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Domain.Entities;
using GymTurnos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymTurnos.Infrastructure.Repositories;

public class CamaRepository : ICamaRepository
{
    private readonly AppDbContext _context;

    public CamaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cama>> GetAllAsync()
    {
        return await _context.Camas.ToListAsync();
    }

    public async Task<Cama?> GetByIdAsync(int id)
    {
        return await _context.Camas.FindAsync(id);
    }

    public async Task<Cama> CreateAsync(Cama cama)
    {
        _context.Camas.Add(cama);
        await _context.SaveChangesAsync();

        return cama;
    }

    public async Task<Cama?> UpdateAsync(Cama cama)
    {
        var camaExistente = await _context.Camas.FindAsync(cama.Id);

        if (camaExistente == null)
            return null;

        camaExistente.Numero = cama.Numero;
        camaExistente.Disponible = cama.Disponible;

        await _context.SaveChangesAsync();

        return camaExistente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cama = await _context.Camas.FindAsync(id);

        if (cama == null)
            return false;

        _context.Camas.Remove(cama);
        await _context.SaveChangesAsync();

        return true;
    }
}