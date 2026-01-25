using BackendApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BackendApi.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<User?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Studies)
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}