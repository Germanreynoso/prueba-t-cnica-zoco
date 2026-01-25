using BackendApi.Models;

namespace BackendApi.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByIdWithDetailsAsync(int id);
}