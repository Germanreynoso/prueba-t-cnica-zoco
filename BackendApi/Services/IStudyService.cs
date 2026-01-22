using BackendApi.DTOs;
using BackendApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendApi.Services;

public interface IStudyService
{
    Task<IEnumerable<Study>> GetAllStudiesAsync();
    Task<Study?> GetStudyByIdAsync(int id);
    Task<Study> CreateStudyAsync(CreateStudyDto dto);
    Task UpdateStudyAsync(int id, UpdateStudyDto dto);
    Task DeleteStudyAsync(int id);
}