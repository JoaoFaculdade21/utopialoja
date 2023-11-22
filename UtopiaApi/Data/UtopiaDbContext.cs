using UtopiaAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace UtopiaAPI.Data;

public class UtopiaDbContext : DbContext{
    public DbSet<Categoria>? Categoria { get; set;}
    public DbSet<Produto>? Produto { get; set; }
    public DbSet<Carrinho>? Carrinho { get; set; }
    public DbSet<Cliente>? Cliente { get; set; }
    public DbSet<ItemCarrinho>? ItemCarrinho { get; set; }
    public DbSet<Pedido>? Pedido { get; set; }
    public DbSet<Pagamento>? Pagamento { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=utopia.db;Cache=Shared");
    }
}