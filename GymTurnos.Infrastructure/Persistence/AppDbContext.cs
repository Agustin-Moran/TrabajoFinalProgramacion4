using System;
using System.Collections.Generic;
using System.Text;

using GymTurnos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymTurnos.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Profesor> Profesores { get; set; }
    public DbSet<Administrador> Administradores { get; set; }
    public DbSet<Clase> Clases { get; set; }
    public DbSet<Cama> Camas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
}