namespace webProductos.Application.Dtos;

public class AuthResponseDto
{
    public string Token { get; set; }

    public AuthResponseDto(string token)
    {
        Token = token;
    }
}