using BackendApi.DTOs;
using BackendApi.Models;
using BackendApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IStudyRepository _studyRepository;
    private readonly IAddressRepository _addressRepository;

    public UserService(
        IUserRepository userRepository,
        IStudyRepository studyRepository,
        IAddressRepository addressRepository)
    {
        _userRepository = userRepository;
        _studyRepository = studyRepository;
        _addressRepository = addressRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<User> CreateUserAsync(CreateUserDto dto)
    {
        var existingUsers = await _userRepository.GetAllAsync();
        if (existingUsers.Any(u => u.Username == dto.Username || u.Email == dto.Email))
        {
            throw new InvalidOperationException("Username or email already exists.");
        }

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };

        return await _userRepository.AddAsync(user);
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (!string.IsNullOrEmpty(dto.Username) && dto.Username != user.Username)
        {
            var existingUsers = await _userRepository.GetAllAsync();
            if (existingUsers.Any(u => u.Username == dto.Username))
            {
                throw new InvalidOperationException("Username already exists.");
            }
            user.Username = dto.Username;
        }

        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
        {
            var existingUsers = await _userRepository.GetAllAsync();
            if (existingUsers.Any(u => u.Email == dto.Email))
            {
                throw new InvalidOperationException("Email already exists.");
            }
            user.Email = dto.Email;
        }

        if (!string.IsNullOrEmpty(dto.FirstName)) user.FirstName = dto.FirstName;
        if (!string.IsNullOrEmpty(dto.LastName)) user.LastName = dto.LastName;
        if (dto.Role.HasValue) user.Role = dto.Role.Value;

        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        await _userRepository.DeleteAsync(id);
    }

    public async Task<bool> IsOwnerAsync(int userId, string resourceType, int resourceId)
    {
        switch (resourceType)
        {
            case "User":
                return userId == resourceId;
            case "Study":
                var study = await _studyRepository.GetByIdAsync(resourceId);
                return study != null && study.UserId == userId;
            case "Address":
                var address = await _addressRepository.GetByIdAsync(resourceId);
                return address != null && address.UserId == userId;
            default:
                return false;
        }
    }
}