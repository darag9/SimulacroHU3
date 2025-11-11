using System.Security.Cryptography;
using System.Text;
using Moq;
using webProductos.Application.Contracts;
using webProductos.Application.Dtos;
using webProductos.Application.Services;
using webProductos.Domain.Entities;
using webProductos.Domain.Interfaces;
using Xunit;

namespace webProductos.Tests.Tests;

public class AuthServiceTests
{
    
    private readonly Mock<IUserRepository> _mockRepo = new Mock<IUserRepository>();
    private readonly Mock<IJwtTokenGenerator> _mockTokenGenerator = new Mock<IJwtTokenGenerator>();
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(_mockRepo.Object, _mockTokenGenerator.Object);
    }
    
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
    
    //Test 1: Verificacion de login de ususario
    [Fact]
    public async Task LoginAsync_ReturnToken_WhenCredentialsAreValid()
    {
        var password = "ValidPassword123!";
        var loginDto = new LoginDto { Username = "testuser", Password = "ValidPassword123!" };
        var expectedToken = "fake-jwt-token";

        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        
        _mockRepo.Setup(r => r.GetByUsernameAsync(loginDto.Username))
            .ReturnsAsync(new User { 
                Username = "testuser", 
                PasswordHash = passwordHash, 
                PasswordSalt = passwordSalt 
            });
        
        _mockTokenGenerator.Setup(g => g.GenerateToken(It.IsAny<User>()))
            .Returns(expectedToken);
        
        var result = await _authService.LoginAsync(loginDto);
        
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
        _mockRepo.Verify(r => r.GetByUsernameAsync("testuser"), Times.Once);
        
    }
    
    [Fact]
    public async Task LoginAsync_ThrowException_WhenUserNotFound()
    {
        var loginDto = new LoginDto { Username = "nonexistent", Password = "password" };
        
        _mockRepo.Setup(r => r.GetByUsernameAsync(loginDto.Username))
            .ReturnsAsync((User)null!);
        
        await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(loginDto));
    }
}
