using Microsoft.EntityFrameworkCore;
using webProductos.Domain.Entities;

namespace webProductos.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product>  Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
                
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
                
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();
        });
    }
}