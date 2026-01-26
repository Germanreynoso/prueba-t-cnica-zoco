using System.ComponentModel.DataAnnotations;

namespace BackendApi.DTOs;

public class LoginDto
{
    [Required]
    [MaxLength(100)]
    public required string UsernameOrEmail { get; set; }

    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
}