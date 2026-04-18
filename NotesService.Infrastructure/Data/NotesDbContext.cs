using Microsoft.EntityFrameworkCore;
using NotesService.Domain.Entities;

namespace NotesService.Infrastructure.Data;

public class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

    public DbSet<Note> Notes { get; set; }
    public DbSet<ApiLog> ApiLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Aquí podrías configurar más detalles de las tablas
    }
}