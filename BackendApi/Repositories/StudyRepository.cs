using BackendApi.Models;

namespace BackendApi.Repositories;

public class StudyRepository : Repository<Study>, IStudyRepository
{
    public StudyRepository(ApplicationDbContext context) : base(context) { }
}