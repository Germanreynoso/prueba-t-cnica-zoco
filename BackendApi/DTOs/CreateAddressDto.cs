using System.ComponentModel.DataAnnotations;

namespace BackendApi.DTOs;

public class CreateAddressDto
{
    [Required]
    [MaxLength(200)]
    public required string Street { get; set; }

    [Required]
    [MaxLength(100)]
    public required string City { get; set; }

    [MaxLength(50)]
    public string? State { get; set; }

    [MaxLength(20)]
    public string? ZipCode { get; set; }

    [MaxLength(50)]
    public string? Country { get; set; }

    [Required]
    public int UserId { get; set; }
}