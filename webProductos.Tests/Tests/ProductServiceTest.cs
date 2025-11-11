using Moq;
using webProductos.Application.Contracts;
using webProductos.Application.Dtos;
using webProductos.Application.Services;
using webProductos.Domain.Entities;
using webProductos.Domain.Interfaces;
using Xunit;

namespace webProductos.Tests.Tests;

public class ProductServiceTest
{
    private readonly Mock<IProductRepository> _mockRepo = new Mock<IProductRepository>();
    private readonly IProductService _productService;

    public ProductServiceTest()
    {
        _productService = new ProductService(_mockRepo.Object);
    }
    
    //test 2: Validacion de creacion de producto.
    [Fact]
    public async Task CreateProductAsync_ReturnProductDto_WhenValidDAtaIsProvided()
    {
        var createDto = new CreateProductDto
        {
            Name = "Test Product",
            Description = "Description",
            Price = 10.50m,
            Stock = 100 
        };

        var result = await _productService.CreateProductAsync(createDto);
        
        Assert.NotNull(result);
        Assert.Equal("Test Product", result.Name);
        
        
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }
    
}