using System.ComponentModel.DataAnnotations;

namespace webProductos.Application.Dtos;

public class UpdateUserDto
{
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El rol es obligatorio.")]
    public string Role { get; set; }
}