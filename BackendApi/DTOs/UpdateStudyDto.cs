using System.ComponentModel.DataAnnotations;

namespace BackendApi.DTOs;

public class UpdateStudyDto
{
    [MaxLength(200)]
    public string Title { get; set; }

    public string Description { get; set; }
}