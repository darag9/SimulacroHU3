using webProductos.Application.Dtos;

namespace webProductos.Application.Contracts;

public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
    Task UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    Task DeleteProductAsync(int id);
}