using BackendApi.DTOs;
using BackendApi.Models;
using BackendApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Services;

public class StudyService : IStudyService
{
    private readonly IStudyRepository _studyRepository;
    private readonly IUserRepository _userRepository;

    public StudyService(IStudyRepository studyRepository, IUserRepository userRepository)
    {
        _studyRepository = studyRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<Study>> GetAllStudiesAsync()
    {
        return await _studyRepository.GetAllAsync();
    }

    public async Task<Study?> GetStudyByIdAsync(int id)
    {
        return await _studyRepository.GetByIdAsync(id);
    }

    public async Task<Study> CreateStudyAsync(CreateStudyDto dto)
    {
        // Check if user exists
        var user = await _userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var study = new Study
        {
            Title = dto.Title,
            Description = dto.Description,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        return await _studyRepository.AddAsync(study);
    }

    public async Task UpdateStudyAsync(int id, UpdateStudyDto dto)
    {
        var study = await _studyRepository.GetByIdAsync(id);
        if (study == null)
        {
            throw new KeyNotFoundException("Study not found.");
        }

        if (!string.IsNullOrEmpty(dto.Title)) study.Title = dto.Title;
        if (!string.IsNullOrEmpty(dto.Description)) study.Description = dto.Description;

        await _studyRepository.UpdateAsync(study);
    }

    public async Task DeleteStudyAsync(int id)
    {
        var study = await _studyRepository.GetByIdAsync(id);
        if (study == null)
        {
            throw new KeyNotFoundException("Study not found.");
        }

        await _studyRepository.DeleteAsync(id);
    }
}