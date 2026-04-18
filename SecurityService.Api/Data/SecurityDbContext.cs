using Microsoft.EntityFrameworkCore;
using SecurityService.Domain.Entities;

namespace SecurityService.Api.Data;

public class SecurityDbContext : DbContext
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuraciones adicionales si son necesarias
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
    }
}
