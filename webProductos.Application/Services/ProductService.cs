using webProductos.Application.Contracts;
using webProductos.Application.Dtos;
using webProductos.Domain.Entities;
using webProductos.Domain.Interfaces;

namespace webProductos.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository  _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            throw new Exception("Producto no encontrado");
        }
        
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        
        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        });
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            Stock = createProductDto.Stock
        };
        
        await _productRepository.AddAsync(product);
        
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
    }

    public async Task UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        var productToUpdate = await _productRepository.GetByIdAsync(id);
        
        if (productToUpdate == null)
        {
            throw new Exception($"Producto con id {id} no encontrado.");
        }
        
        productToUpdate.Name = updateProductDto.Name;
        productToUpdate.Description = updateProductDto.Description;
        productToUpdate.Price = updateProductDto.Price;
        productToUpdate.Stock = updateProductDto.Stock;
        
        await _productRepository.UpdateAsync(productToUpdate);
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            throw new Exception($"Producto con id {id} no encontrado.");
        }
        
        await _productRepository.DeleteAsync(id);
    }
}