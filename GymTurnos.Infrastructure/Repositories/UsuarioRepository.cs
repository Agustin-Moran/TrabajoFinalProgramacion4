using GymTurnos.Application.Interfaces.Repositories;
using GymTurnos.Domain.Entities;
using GymTurnos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymTurnos.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<Usuario?> UpdateAsync(Usuario usuario)
    {
        var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id);

        if (usuarioExistente == null)
            return null;

        usuarioExistente.Nombre = usuario.Nombre;
        usuarioExistente.Apellido = usuario.Apellido;
        usuarioExistente.Email = usuario.Email;
        usuarioExistente.PasswordHash = usuario.PasswordHash;
        usuarioExistente.Activo = usuario.Activo;
        usuarioExistente.Rol = usuario.Rol;

        await _context.SaveChangesAsync();

        return usuarioExistente;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return true;
    }
}