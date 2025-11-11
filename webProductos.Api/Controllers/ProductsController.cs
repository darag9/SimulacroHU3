using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webProductos.Application.Contracts;
using webProductos.Application.Dtos;

namespace webProductos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // -> /api/products
    [Authorize] // ¡Importante!
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet] // GET /api/products
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")] // GET /api/products/5
        public async Task<IActionResult> GetProductById(int id)
        {

            Console.WriteLine($"Recibiendo {id}");
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                Console.WriteLine("--- CONTROLADOR: Producto recibido del servicio. Enviando 200 OK ---");
                return Ok(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--- CONTROLADOR: ¡ERROR! Se atrapó una excepción: {ex.Message} ---");
                Console.WriteLine(ex.StackTrace);
                return NotFound(ex.Message);
            }
        }

        [HttpPost] // POST /api/products
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            var newProduct = await _productService.CreateProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id:int}")] // PUT /api/products/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            try
            {
                await _productService.UpdateProductAsync(id, updateProductDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:int}")] // DELETE /api/products/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}