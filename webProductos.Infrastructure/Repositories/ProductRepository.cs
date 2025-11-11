using Microsoft.EntityFrameworkCore;
using webProductos.Domain.Entities;
using webProductos.Domain.Interfaces;
using webProductos.Infrastructure.Data;

namespace webProductos.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product> GetByIdAsync(int id)
    {
        Console.WriteLine($"--- REPOSITORIO: Buscando producto con ID: {id} ---");

        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        
        if (product == null)
        {
            Console.WriteLine("--- REPOSITORIO: No se encontró nada (null) ---");
        }
        else
        {
            Console.WriteLine($"--- REPOSITORIO: Producto encontrado: {product.Name} ---");
        }

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        
    }
}