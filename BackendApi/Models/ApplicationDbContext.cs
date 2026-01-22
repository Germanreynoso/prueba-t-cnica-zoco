using Microsoft.EntityFrameworkCore;

namespace BackendApi.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Study> Studies { get; set; }
    public DbSet<SessionLog> SessionLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Admin User
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 1,
            Username = "admin",
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            FirstName = "Admin",
            LastName = "System",
            Role = UserRole.Admin,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        // Seed Regular User
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = 2,
            Username = "usuario",
            Email = "user@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
            FirstName = "Juan",
            LastName = "Perez",
            Role = UserRole.User,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}