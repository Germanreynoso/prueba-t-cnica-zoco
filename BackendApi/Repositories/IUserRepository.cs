using BackendApi.Models;

namespace BackendApi.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByIdWithDetailsAsync(int id);
    Task<(IEnumerable<User> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? searchTerm);
}