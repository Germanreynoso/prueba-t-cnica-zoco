using BackendApi.DTOs;
using BackendApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Services;

public class MockStudyService : IStudyService
{
    private static readonly List<Study> _studies = new List<Study>();

    public async Task<IEnumerable<Study>> GetAllStudiesAsync() => _studies;

    public async Task<Study?> GetStudyByIdAsync(int id) => _studies.FirstOrDefault(s => s.Id == id);

    public async Task<Study> CreateStudyAsync(CreateStudyDto dto)
    {
        var study = new Study
        {
            Id = _studies.Count > 0 ? _studies.Max(s => s.Id) + 1 : 1,
            Title = dto.Title,
            Description = dto.Description,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow
        };
        _studies.Add(study);
        return study;
    }

    public async Task UpdateStudyAsync(int id, UpdateStudyDto dto)
    {
        var study = _studies.FirstOrDefault(s => s.Id == id);
        if (study != null)
        {
            if (!string.IsNullOrEmpty(dto.Title)) study.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description)) study.Description = dto.Description;
        }
    }

    public async Task DeleteStudyAsync(int id)
    {
        var study = _studies.FirstOrDefault(s => s.Id == id);
        if (study != null) _studies.Remove(study);
    }
}
