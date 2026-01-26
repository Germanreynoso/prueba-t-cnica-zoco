using BackendApi.DTOs;
using BackendApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<PaginatedResult<User>> GetUsersPaginatedAsync(int page, int pageSize, string? searchTerm);
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(CreateUserDto dto);
    Task UpdateUserAsync(int id, UpdateUserDto dto);
    Task DeleteUserAsync(int id);
    Task<bool> IsOwnerAsync(int userId, string resourceType, int resourceId);
}