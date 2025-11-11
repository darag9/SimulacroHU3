using webProductos.Domain.Entities;

namespace webProductos.Application.Contracts;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}