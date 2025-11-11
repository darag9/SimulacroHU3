using System.ComponentModel.DataAnnotations;

namespace webProductos.Application.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es valido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string Password { get; set; }
    
    public string Role { get; set; }
}