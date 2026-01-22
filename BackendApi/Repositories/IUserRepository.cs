using BackendApi.Models;

namespace BackendApi.Repositories;

public interface IUserRepository : IRepository<User>
{
    // Add specific methods if needed, e.g., Task<User?> GetByUsernameAsync(string username);
}