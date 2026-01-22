using BackendApi.DTOs;
using BackendApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Services;

public class MockUserService : IUserService
{
    private readonly List<User> _users;

    public MockUserService(AuthService authService)
    {
        _users = authService.GetMockUsers();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync() => _users;

    public async Task<User?> GetUserByIdAsync(int id) => _users.FirstOrDefault(u => u.Id == id);

    public async Task<User> CreateUserAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Id = _users.Max(u => u.Id) + 1,
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = dto.Password, // No hashing for mock
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };
        _users.Add(user);
        return user;
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.FirstName)) user.FirstName = dto.FirstName;
            if (!string.IsNullOrEmpty(dto.LastName)) user.LastName = dto.LastName;
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null) _users.Remove(user);
    }

    public async Task<bool> IsOwnerAsync(int userId, string resourceType, int resourceId)
    {
        return true; // Bypass for mock
    }
}
