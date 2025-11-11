using System.ComponentModel.DataAnnotations;

namespace webProductos.Application.Dtos;

public class UpdateProductDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "El stock es obligatorio.")]
    public int Stock { get; set; }
}