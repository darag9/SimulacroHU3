using System.Security.Cryptography;
using System.Text;
using webProductos.Application.Contracts;
using webProductos.Application.Dtos;
using webProductos.Domain.Entities;
using webProductos.Domain.Interfaces;

namespace webProductos.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.GetByEmailAsync(registerDto.Email) != null)
        {
            throw new Exception($"El email {registerDto.Email} ya existe.");
        }

        if (await _userRepository.GetByUsernameAsync(registerDto.Username) != null)
        {
            throw new Exception($"El username {registerDto.Username} ya existe.");
        }
        
        CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = string.IsNullOrEmpty(registerDto.Role) ? "User" : registerDto.Role
        };
        
        await _userRepository.AddAsync(user);
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthResponseDto(token);
    }

    
    
    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user == null || !VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new Exception($"Credenciales Incorrectas.");
        }
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthResponseDto(token);
    }
    
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}