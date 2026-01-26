using System.ComponentModel.DataAnnotations;
using BackendApi.Models;

namespace BackendApi.DTOs;

public class UpdateUserDto
{
    [MaxLength(50)]
    public string? Username { get; set; }

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    public UserRole? Role { get; set; }
}