using System.ComponentModel.DataAnnotations;
using BackendApi.Models;

namespace BackendApi.DTOs;

public class CreateUserDto
{
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; } // Plain password, will be hashed in service

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [Required]
    public UserRole Role { get; set; }
}