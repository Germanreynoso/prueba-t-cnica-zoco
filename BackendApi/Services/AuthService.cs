using BackendApi.Models;
using BackendApi.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace BackendApi.Services;

public class AuthService
{
    // Hardcoded users for testing without DB
    private static readonly List<User> _users = new List<User>
    {
        new User { Id = 1, Username = "admin", Email = "admin@example.com", PasswordHash = "admin123", FirstName = "Admin", LastName = "System", Role = UserRole.Admin },
        new User { Id = 2, Username = "usuario", Email = "user@example.com", PasswordHash = "user123", FirstName = "Juan", LastName = "Perez", Role = UserRole.User }
    };

    public string GenerateJwtToken(User user)
    {
        // Return a dummy token for now
        return "dummy-token-for-testing";
    }

    public User Authenticate(LoginDto loginDto)
    {
        // Simple check without hashing for now
        return _users.FirstOrDefault(u => 
            (u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail) 
            && u.PasswordHash == loginDto.Password);
    }

    public List<User> GetMockUsers() => _users;
}