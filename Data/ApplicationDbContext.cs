// Contexto principal de Entity Framework Core
// Representa la conexión y las tablas de la base de datos
using AIHubBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace AIHubBackend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Tool> Tools => Set<Tool>();
}