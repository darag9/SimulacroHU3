using webProductos.Application.Contracts;
using webProductos.Application.Dtos;
using webProductos.Domain.Interfaces;

namespace webProductos.Application.Services;

public class UserService : IUserService
{
    
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new Exception($"Usuario con id {id} no encontrado.");
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
            
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        });
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var userToUpdate = await _userRepository.GetByIdAsync(id);
        if (userToUpdate == null)
        {
            throw new Exception($"Usuario con id {id} no encontrado.");
        }
            
        var existingUserWithEmail = await _userRepository.GetByEmailAsync(updateUserDto.Email);
        if (existingUserWithEmail != null && existingUserWithEmail.Id != id)
        {
            throw new Exception($"El email '{updateUserDto.Email}' ya está en uso.");
        }

        userToUpdate.Email = updateUserDto.Email;
        userToUpdate.Role = updateUserDto.Role;

        await _userRepository.UpdateAsync(userToUpdate);
    }

    public async Task DeleteUserAsync(int id)
    {
        var userToDelete = await _userRepository.GetByIdAsync(id);
        if (userToDelete == null)
        {
            throw new Exception($"Usuario con id {id} no encontrado.");
        }

        await _userRepository.DeleteAsync(id);
    }
}