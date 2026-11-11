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

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Clase> Clases { get; set; }
    public DbSet<Cama> Camas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Cliente)
            .WithMany()
            .HasForeignKey(r => r.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Cama)
            .WithMany()
            .HasForeignKey(r => r.CamaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Clase>()
            .HasOne(c => c.Profesor)
            .WithMany()
            .HasForeignKey(c => c.ProfesorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}