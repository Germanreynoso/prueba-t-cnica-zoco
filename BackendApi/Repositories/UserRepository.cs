using BackendApi.Models;

namespace BackendApi.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    // Implement specific methods if added to interface
}