using Microsoft.AspNetCore.Mvc;
using webProductos.Application.Contracts;
using webProductos.Application.Dtos;

namespace webProductos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var authResponse = await _authService.RegisterAsync(registerDto);
            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var authResponse = await _authService.LoginAsync(loginDto);
                
            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}