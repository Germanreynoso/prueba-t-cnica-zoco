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

    public async Task<(IEnumerable<User> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? searchTerm)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var lowerTerm = searchTerm.ToLower();
            query = query.Where(u => u.Username.ToLower().Contains(lowerTerm) || 
                                     u.Email.ToLower().Contains(lowerTerm) || 
                                     (u.FirstName != null && u.FirstName.ToLower().Contains(lowerTerm)) || 
                                     (u.LastName != null && u.LastName.ToLower().Contains(lowerTerm)));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}