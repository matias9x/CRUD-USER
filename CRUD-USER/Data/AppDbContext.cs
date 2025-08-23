using Microsoft.EntityFrameworkCore;
using CRUD_USER.Entities;

namespace CRUD_USER.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Produto> Produtos { get; set; } = null!; //adicionando o dbset do produto
    public DbSet<Fornecedor> Fornecedores { get; set; } = null!; //adicionando o dbset do fornecedor 
}

