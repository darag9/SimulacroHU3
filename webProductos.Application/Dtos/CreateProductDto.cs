using System.ComponentModel.DataAnnotations;

namespace webProductos.Application.Dtos;

public class CreateProductDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, 1000000.00, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "El stock es obligatorio.")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }
}