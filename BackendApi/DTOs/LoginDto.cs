using System.ComponentModel.DataAnnotations;

namespace BackendApi.DTOs;

public class LoginDto
{
    [Required]
    [MaxLength(100)]
    public string UsernameOrEmail { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}