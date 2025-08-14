using Microsoft.EntityFrameworkCore;
using CRUD_USER.Entities;

namespace CRUD_USER.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=localhost,1433;Database=sqlserver;User ID=sa;Password=kalunga1908081087;Trusted_Connection=False; TrustServerCertificate=True;");
}

