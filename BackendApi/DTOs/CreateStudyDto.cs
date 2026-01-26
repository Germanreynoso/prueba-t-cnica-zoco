using System.ComponentModel.DataAnnotations;

namespace BackendApi.DTOs;

public class CreateStudyDto
{
    [Required]
    [MaxLength(200)]
    public required string Title { get; set; }

    public string? Description { get; set; }

    [Required]
    public int UserId { get; set; }
}